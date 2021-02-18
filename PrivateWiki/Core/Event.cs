using System;
using System.Reactive.Subjects;

namespace PrivateWiki.Core
{
	public abstract class Event<T> : IObserver<T>, IObservable<T>
	{
		private readonly ISubject<T> _subject;

		public Event()
		{
			_subject = new Subject<T>();
		}

		public void OnCompleted()
		{
			_subject.OnCompleted();
		}

		public void OnError(Exception error)
		{
			_subject.OnError(error);
		}

		public void OnNext(T value)
		{
			_subject.OnNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			return _subject.Subscribe(observer);
		}
	}
}