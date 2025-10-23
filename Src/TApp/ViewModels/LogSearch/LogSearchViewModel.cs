using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using TApp.Apis;
using TApp.Apis.Models;
using VisDummy.Shared.Utils;

namespace TApp.ViewModels.LogSearch
{
    public class LogSearchViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ICloudApi _cloudApi;
        public string UrlPathSegment => UrlDefines.URL_LogSearch;
        public IScreen HostScreen { get; }
        public LogSearchViewModel(ICloudApi cloudApi)
        {
            _cloudApi = cloudApi;

            this.HostScreen = Locator.Current.GetRequiredService<IScreen>()!;

            this.CmdLoadResouces = ReactiveCommand.CreateFromTask(LoadResourcesImpl);
            this.CmdLoadResouces.Select(x => x.Total).ToPropertyEx(this, x => x.Total);
            this.CmdLoadResouces.Select(x => x.Current).ToPropertyEx(this, x => x.Current);

            this.CmdNextPage = ReactiveCommand.CreateFromTask(LoadNextPage);
            this.CmdPrevPage = ReactiveCommand.CreateFromTask(LoadPrevPage);

            this.CmdSearch = ReactiveCommand.CreateFromTask(LoadSearch);

            var logLevelList = EnumHelper.GetAllValuesAndDescriptions(typeof(LogLevel)).ToList();
            logLevelList.Insert(0, new ValueDescription { Description = "全部", Value = null });
            EnumValues = new ObservableCollection<ValueDescription>(logLevelList);
        }

        //private string _group;
        //public string Group
        //{
        //    get => _group;
        //    set => this.RaiseAndSetIfChanged(ref _group, value);
        //}
        [Reactive]
        public string Group { get; set; }

        //private string _source;
        //public string Source
        //{
        //    get => _source;
        //    set => this.RaiseAndSetIfChanged(ref _source, value);
        //}
        [Reactive]
        public string Source { get; set; }

        //private string _content;
        //public string Content
        //{
        //    get => _content;
        //    set => this.RaiseAndSetIfChanged(ref _content, value);
        //}
        [Reactive]
        public string Content { get; set; }

        //private LogLevel? _level;
        //public LogLevel? Level
        //{
        //    get => _level;
        //    set => this.RaiseAndSetIfChanged(ref _level, value);
        //}

        [Reactive]
        public LogLevel? Level { get; set; }

        //private DateTime? _startTime;
        //public DateTime? StartTime
        //{
        //    get => _startTime;
        //    set => this.RaiseAndSetIfChanged(ref _startTime, value);
        //}
        [Reactive]
        public DateTime? StartTime { get; set; }

        //private DateTime? _endTime;
        //public DateTime? EndTime
        //{
        //    get => _endTime;
        //    set => this.RaiseAndSetIfChanged(ref _endTime, value);
        //}
        [Reactive]
        public DateTime? EndTime { get; set; }

        public ObservableCollection<ValueDescription> EnumValues { get; } = [];

        public ObservableCollection<LogItemResponse> Resources { get; } = [];

        [ObservableAsProperty]
        public int Total { get; }

        [ObservableAsProperty]
        public int Current { get; }

        public ReactiveCommand<Unit, Unit> CmdSearch { get; }

        public ReactiveCommand<Unit, LogPaginationResponse> CmdLoadResouces { get; }

        public ReactiveCommand<Unit, Unit> CmdNextPage { get; }

        public ReactiveCommand<Unit, Unit> CmdPrevPage { get; }

        private int pageIndex = 1;
        private int pageSize = 20;
        private async Task<LogPaginationResponse> LoadResourcesImpl()
        {
            var startTime = StartTime;
            var endTime = EndTime.HasValue ? EndTime.Value.AddDays(1) : EndTime;

            var result = await this._cloudApi.LogPagination(Source, Group, Level, Content, startTime, endTime, pageIndex, pageSize);
            this.Resources.Clear();
            this.Resources.AddRange(result.List);
            pageIndex = result.Current;
            return result;
        }

        private async Task LoadNextPage()
        {
            if (pageIndex * pageSize > Total) return;
            if (Current != 0) pageIndex++;
            await CmdLoadResouces.Execute();
        }

        private async Task LoadPrevPage()
        {
            if (pageIndex <= 1) return;
            pageIndex--;
            await CmdLoadResouces.Execute();
        }

        private async Task LoadSearch()
        {
            pageIndex = 1;
            await CmdLoadResouces.Execute();
        }

    }
}
