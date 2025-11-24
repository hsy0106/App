using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Interface
{

    public interface IObservable
    {
        void Attach(IObserver observer);

    }
    public interface IObserver
    {
        void Update(IObservable observable);
    }
    public interface IDisplay 
    {
        void Display();
    }
    public class Persion : IObservable
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _name;
        private string _moode;
        private int _energy;

        public string Name { 
            get => _name; 
            set
                {
                _name = value;
                Notify();
            }  
        }
        public string Mood {
            get => _moode;
            set
            {
                _moode = value;
                Notify();
            }
        }

        public int Energy {
            get => _energy;
            set
            {
                _energy = value;
                Notify();
            }
        }
        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }
        public override string ToString()
        {
            return $"{Name} [心情: {Mood}, 精力: {Energy}]";
        }

    }



}
