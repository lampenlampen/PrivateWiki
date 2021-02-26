using System;
using System.Reactive.Subjects;

namespace PrivateWiki.Core
{
	public abstract class Event<TEventArgs> : IObservable<TEventArgs>, ICommandHandler<TEventArgs>
	{
		private readonly ISubject<TEventArgs> _subject;

		public Event()
		{
			_subject = new Subject<TEventArgs>();
		}

		public IDisposable Subscribe(IObserver<TEventArgs> observer)
		{
			return _subject.Subscribe(observer);
		}

		public void Handle(TEventArgs command)
		{
			_subject.OnNext(command);
		}
	}
}