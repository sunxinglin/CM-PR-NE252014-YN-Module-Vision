using Catl.HostComputer.CommonServices.Mes;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Catl.MesInvocation.CatlMesInvoker.Impl
{
    public class ExcelSfcInvocationLogger : ISfcInvocationLogger
    {
        private readonly ISfcLogSettingsProvider _settingProvider;

        private readonly ILogger<ExcelSfcInvocationLogger> _logger;

        private readonly IHostEnvironment _env;

        private ConcurrentDictionary<string, SemaphoreSlim> _signals = new ConcurrentDictionary<string, SemaphoreSlim>();

        public ExcelSfcInvocationLogger(ISfcLogSettingsProvider settingProvider, ILogger<ExcelSfcInvocationLogger> logger, IHostEnvironment env)
        {
            _settingProvider = settingProvider;
            _logger = logger;
            _env = env;
        }

        private int GetTimeout()
        {
            return (_settingProvider.GetSettings() ?? throw new Exception("运行日志设置不可为空！请检查！")).SemaphoreTimeout;
        }

        public string GetLogSheetName()
        {
            return (_settingProvider.GetSettings() ?? throw new Exception("SFC日志设置不可为空！请检查！")).LogSheetName;
        }

        public uint GetLogSheetId()
        {
            return (_settingProvider.GetSettings() ?? throw new Exception("SFC日志设置不可为空！请检查！")).LogSheetId;
        }

        public string GetLogPath(DateTime dt, string svcname, string description, string equipId)
        {
            string text = (_settingProvider.GetSettings() ?? throw new Exception("SFC日志设置不可为空！请检查！")).LogFileBaseDir;
            if (string.IsNullOrEmpty(text))
            {
                text = "D:/" + _env.ApplicationName + "/";
            }

            string text2 = Path.Combine(text, "MESlog", svcname + "_" + description);
            if (!Directory.Exists(text2))
            {
                Directory.CreateDirectory(text2);
            }

            string text3 = $"{dt:yyyyMMdd}.xlsx";
            text3 = (string.IsNullOrEmpty(equipId) ? text3 : (equipId + "_" + text3));
            return Path.Combine(text2, text3);
        }

        private Cell CreateCell(int value)
        {
            return new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue($"{value}")
            };
        }

        private Cell CreateCell(string value)
        {
            return new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(value)
            };
        }

        private Row CreateRow(SheetData sheetData, Cell dt, Cell dd)
        {
            Row row = new Row();
            row.AppendChild(dt);
            row.AppendChild(dd);
            sheetData.AppendChild(row);
            return row;
        }

        public async Task WriteLogAsync(SfcInvocationLogging log, string filepath)
        {
            _ = 2;
            try
            {
                int timeout = GetTimeout();
                _logger.LogDebug("【写SFC调用日志】：开始，日志文件路径=" + filepath);
                if (!(await _signals.GetOrAdd(filepath, (string k) => new SemaphoreSlim(1, 1)).WaitAsync(timeout)))
                {
                    throw new TimeoutException("【写SFC调用日志】：获取日志文件写锁超时。日志内容=" + JsonConvert.SerializeObject(log));
                }

                _logger.LogDebug("【写SFC调用日志】：成功获取锁，日志文件路径=" + filepath);
                if (!File.Exists(filepath))
                {
                    await LogToBrandNewDocAsync(log, filepath);
                }
                else
                {
                    await LogToExistedDocAsync(log, filepath);
                }
            }
            finally
            {
                _logger.LogDebug("【写SFC调用日志】完成：日志文件路径=" + filepath + "，现场开始移除并发锁");
                if (_signals.TryRemove(filepath, out var value))
                {
                    value.Release();
                }

                _logger.LogDebug("【写SFC调用日志】清理：日志文件路径=" + filepath + "，成功移除并发锁");
            }
        }

        private Task LogToExistedDocAsync(SfcInvocationLogging log, string filepath)
        {
            uint sheetId = GetLogSheetId();
            using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filepath, isEditable: true);
            WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
            Sheet sheet = (from sht in workbookPart.Workbook.Descendants<Sheet>()
                           where (uint)sht.SheetId == sheetId
                           select sht).FirstOrDefault();
            SheetData sheetdata = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet.Elements<SheetData>().FirstOrDefault();
            AppendLogRows(sheetdata, log);
            workbookPart.Workbook.Save();
            return Task.CompletedTask;
        }

        private Task LogToBrandNewDocAsync(SfcInvocationLogging log, string filepath)
        {
            using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            SheetData sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);
            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
            Sheet sheet = new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = new UInt32Value(GetLogSheetId()),
                Name = GetLogSheetName()
            };
            sheets.Append(sheet);
            AppendLogRows(sheetData, log);
            workbookPart.Workbook.Save();
            return Task.CompletedTask;
        }

        private void AppendLogRows(SheetData sheetdata, SfcInvocationLogging log)
        {
            if (sheetdata == null)
            {
                throw new ArgumentNullException("sheetdata");
            }

            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            CreateRow(sheetdata, CreateCell("SFC"), CreateCell(log.SfcNumber));
            CreateRow(sheetdata, CreateCell("Interface call start time"), CreateCell($"{log.SentAt:HH:mm:ss:fff}"));
            CreateRow(sheetdata, CreateCell("Interface call parameter passing"), CreateCell(log.Payload));
            CreateRow(sheetdata, CreateCell("Interface call return time"), CreateCell($"{log.ReceivedAt:HH:mm:ss:fff}"));
            double num = Math.Round(log.ElapsedTime.TotalMilliseconds);
            CreateRow(sheetdata, CreateCell("Time-consuming（ms）"), CreateCell((int)num));
            CreateRow(sheetdata, CreateCell("Return code"), CreateCell(log.RespCode ?? ""));
            CreateRow(sheetdata, CreateCell("Return information"), CreateCell(log.RespInfo ?? ""));
            CreateRow(sheetdata, CreateCell(""), CreateCell(""));
        }
    }
}
