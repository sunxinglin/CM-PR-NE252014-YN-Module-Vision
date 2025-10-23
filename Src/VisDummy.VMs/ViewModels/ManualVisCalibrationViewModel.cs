using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using VisDummy.Abstractions.Calibrations;
using VisDummy.Abstractions.Infra;
using VM.Core;
using Unit = System.Reactive.Unit;

namespace VisDummy.VMs.ViewModels
{
    public class CalibrationRecord
    {
        [Reactive]
        public int Nth { get; set; }
        [Reactive]
        public float ImgX { get; set; }
        [Reactive]
        public float ImgY { get; set; }
        [Reactive]
        public float ImgA { get; set; }

        [Reactive]
        public float WldX { get; set; }
        [Reactive]
        public float WldY { get; set; }
        [Reactive]
        public float WldA { get; set; }
    }

    public class ManualVisCalibrationViewModel : ReactiveObject
    {
        private readonly IVisProc _visProc;

        public ManualVisCalibrationViewModel(VMSlnViewModel slnVM, IVisProc visProc)
        {
            this.SlnVM = slnVM;
            this._visProc = visProc;
            this.WhenAnyValue(x => x.CurrentProc).Subscribe(curr => {
                if (this.OnProcedureSelected != null)
                {
                    try
                    {
                        this.OnProcedureSelected.Invoke(this.CurrentProc);
                    }
                    catch(Exception ex) 
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });


            this.AddStepA = ReactiveCommand.Create(() => {
                this.WldA += this.StepA;
            });
            this.AddStepX = ReactiveCommand.Create(() => {
                this.WldX += this.StepX;
            });
            this.AddStepY = ReactiveCommand.Create(() => {
                this.WldY += this.StepY;
            });


            this.MinusStepA = ReactiveCommand.Create(() => {
                this.WldA -= this.StepA;
            });
            this.MinusStepX = ReactiveCommand.Create(() => {
                this.WldX -= this.StepX;
            });
            this.MinusStepY = ReactiveCommand.Create(() => {
                this.WldY -= this.StepY;
            });

            this.CalibrateNthPoint = ReactiveCommand.CreateFromTask((int nth) =>this.CalibrateNthPointImpl(nth));

            this.CmdClearCalibrationPoints = ReactiveCommand.Create(() => this.CalibrationRecords.Clear());
        }

        public VMSlnViewModel SlnVM { get; }

        /// <summary>
        /// 当前流程
        /// </summary>
        [Reactive]
        public VmProcedure CurrentProc { get; set; }

        /// <summary>
        /// 用户选定流程的钩子
        /// </summary>
        public OnProcedureSelected OnProcedureSelected { get; set; }


        [Reactive]
        public float StepX { get; set; } = 50.0f;

        [Reactive]
        public float StepY { get; set; } = 50.0f;

        [Reactive]
        public float StepA { get; set; } = 5.0f;

        [Reactive]
        public float WldX { get; set; }
        [Reactive]
        public float WldY { get; set; }
        [Reactive]
        public float WldA { get; set; }

        #region
        public ObservableCollection<CalibrationRecord> CalibrationRecords { get; } = new ObservableCollection<CalibrationRecord>();

        [Reactive]
        public bool AllowEditRecords { get; set; } = false;
        #endregion

        public ReactiveCommand<Unit, Unit> AddStepX { get; }
        public ReactiveCommand<Unit, Unit> AddStepY { get; }
        public ReactiveCommand<Unit, Unit> AddStepA { get; }

        public ReactiveCommand<Unit, Unit> MinusStepX { get; }
        public ReactiveCommand<Unit, Unit> MinusStepY { get; }
        public ReactiveCommand<Unit, Unit> MinusStepA { get; }

        #region
        public async Task CalibrateNthPointImpl(int nth)
        {
            var args = new CalibrateNthPointArgs() { 
                Nth = nth,
                WldA = this.WldA,
                WldX = this.WldX,
                WldY = this.WldY,
            };
            var res = await this._visProc.CalibrateAsync(args);
            if (res.IsError)
            {
                var err = res.ErrorValue;
                throw new Exception(err.ToString());
            }

            var wrap = res.ResultValue;

            this.CalibrationRecords.Add(new CalibrationRecord {
                Nth = args.Nth,
                WldA = args.WldA,
                WldX = args.WldX,
                WldY = args.WldY,

                ImgA = wrap.ImgA,
                ImgX = wrap.ImgX,
                ImgY = wrap.ImgY,
            });
            // ...
            return;
        }
        public ReactiveCommand<int, Unit> CalibrateNthPoint { get; }

        public ReactiveCommand<Unit, Unit> CmdClearCalibrationPoints { get; }
        #endregion
    }
}
