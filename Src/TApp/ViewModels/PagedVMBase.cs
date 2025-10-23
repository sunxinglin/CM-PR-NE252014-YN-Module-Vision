using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using StdUnit.One.Shared;
using StdUnit.Zero.Shared;

namespace TApp.ViewModels
{
    public abstract class PagedVMBase<TRow, TApi>: ReactiveObject
    {
        public virtual TApi Api { get; }
        protected readonly MediatR.IMediator _mediator;

        protected abstract Task<PagedResp<TRow>> LoadTableAsync();

        public PagedVMBase(TApi api, MediatR.IMediator mediator)
        {
            this.Api = api;
            this._mediator = mediator;
            this.PageIndex = 1;
            this.PageSize = 10;

            this.WhenAnyValue(x => x.TableResp)
                .Select(t => t == null ? 0 : (int)Math.Ceiling(t.Total * 1.0 / this.PageSize))
                .ToPropertyEx(this, x => x.TotalPages);

            this.WhenAnyValue(x => x.TableResp)
                .Select(t => t?.Data)
                .ToPropertyEx(this, x => x.Records);

            this.CmdLoad = ReactiveCommand.CreateFromTask<Unit, PagedResp<TRow>>(u => this.LoadTableAsync());

            this.CmdLoad.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(async table => { 
                    try
                    {
                        this.TableResp = table;
                    }
                    catch (Exception ex)
                    {
                        await this.PublishErrorNotification(ex);
                    }
                });


            var canLoadNext = this.WhenAnyValue(x => x.PageIndex).CombineLatest(this.WhenAnyValue(x => x.TotalPages))
                .Select(arg => {
                    var (fst, snd) = arg;
                    return fst < snd;
                });

            this.CmdNextPage = ReactiveCommand.CreateFromTask<Unit, PagedResp<TRow>>(async u => { 
                    this.PageIndex = this.PageIndex + 1;
                    var table = await this.LoadTableAsync();
                    return table;
                }, canLoadNext);

            this.CmdNextPage.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(async table => {
                    try
                    {
                        this.TableResp = table;
                    }
                    catch (Exception ex)
                    {
                        await this.PublishErrorNotification(ex);
                    }
                });

            var canLoadPrev = this.WhenAnyValue(x => x.PageIndex).Select(x => x > 1);
            this.CmdPrevPage = ReactiveCommand.CreateFromTask<Unit, PagedResp<TRow>>(
                async u => {
                    var page = this.PageIndex - 1;
                    page = page < 1 ? 1 : page;
                    this.PageIndex = page;
                    var table = await this.LoadTableAsync();
                    return table;
                }, 
                canLoadPrev
            );

            this.CmdPrevPage.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(async table => {
                    try
                    {
                        this.TableResp = table;
                    }
                    catch (Exception ex)
                    {
                        await this.PublishErrorNotification(ex);
                    }
                });

        }

        protected virtual async Task PublishErrorNotification(Exception ex)
        {
            var src = $"{typeof(TApi)}";
            var msg = new AlarmMessage()
            {
                Content = $"加载{typeof(TRow)}：{ex.Message}\r\n{ex.StackTrace}",
                EventSource = src,
                EventNumber = src,
                Level = Microsoft.Extensions.Logging.LogLevel.Error,
                Timestamp = DateTime.Now,
            };
            await this._mediator.Publish(new UILogNotification(msg));
        }

        [Reactive]
        public virtual int PageIndex { get; set; }
        [Reactive]
        public virtual int PageSize { get; set; }

        [Reactive]
        public virtual PagedResp<TRow> TableResp { get; set; }

        [ObservableAsProperty]
        public virtual IList<TRow> Records { get; }

        [ObservableAsProperty]
        public virtual int TotalPages { get; set; }
        [Reactive]
        public virtual TRow CurrentSelected { get; set; }

        public virtual ReactiveCommand<Unit, PagedResp<TRow>> CmdLoad { get; }
        public virtual ReactiveCommand<Unit, PagedResp<TRow>> CmdPrevPage { get; }
        public virtual ReactiveCommand<Unit, PagedResp<TRow>> CmdNextPage { get; }

    }


}
