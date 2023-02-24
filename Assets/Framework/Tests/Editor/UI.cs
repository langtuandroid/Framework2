﻿using System;
using Framework;
using NSubstitute;
using NUnit.Framework;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tests
{
    public class TestView : IComponentEvent, IFieldChangeCb<string>, IComponentEvent<int>, IFieldChangeCb<int>
    {
        private UnityEvent TestEvent = new UnityEvent();
        private UnityEvent<int> EventWithPara = new Dropdown.DropdownEvent();

        UnityEvent IComponentEvent.GetComponentEvent()
        {
            return TestEvent;
        }

        public string Name;
        public int Age;

        public Action<string> GetFieldChangeCb()
        {
            return (value) => Name = value;
        }

        UnityEvent<int> IComponentEvent<int>.GetComponentEvent()
        {
            return EventWithPara;
        }

        Action<int> IFieldChangeCb<int>.GetFieldChangeCb()
        {
            return value => Age = value;
        }
    }

    public interface BindCommand
    {
        void Func();
        void FuncWithPara();
    }

    public class UI测试
    {
        [Test]
        public void BindCommand()
        {
            IComponentEvent view = new TestView();
            bool isTrigger = false;
            BindFactory factory = new BindFactory(view);

            void Func()
            {
                isTrigger = !isTrigger;
            }

            factory.BindCommand(view, Func);
            factory.Reset();
            view.GetComponentEvent().Invoke();
            Assert.IsTrue(isTrigger);
        }

        [Test]
        public void Bind带参Command()
        {
            IComponentEvent<int> view = new TestView();
            int result = -1;
            BindFactory factory = new BindFactory(view);

            void Func(int a)
            {
                result = a;
            }

            factory.BindCommand<IComponentEvent<int>, int>(view, Func);
            view.GetComponentEvent().Invoke(2);
            Assert.IsTrue(result == 2);
        }

        [Test]
        public void BindProperty()
        {
            TestView view = new TestView();
            BindFactory factory = new BindFactory(view);
            ObservableProperty<string> property = Substitute.For<ObservableProperty<string>>("哈哈");
            factory.Bind(view, property);
            Assert.IsTrue(view.Name == "哈哈");
            property.Value = "修改";
            Assert.IsTrue(view.Name == "修改");
        }

        [Test]
        public void BindProperty不同类型wrap()
        {
            TestView view = new TestView();
            BindFactory factory = new BindFactory(view);
            ObservableProperty<int> property = Substitute.For<ObservableProperty<int>>(1);
            factory.Bind(view, property, (field) => field + "wrap");
            Assert.IsTrue(view.Name == "1wrap");
            property.Value = 2;
            Assert.IsTrue(view.Name == "2wrap");
        }

        [Test]
        public void BindProperty多个字段()
        {
            TestView view = new TestView();
            BindFactory factory = new BindFactory(view);
            ObservableProperty<string> property1 = Substitute.For<ObservableProperty<string>>("p1");
            ObservableProperty<string> property2 = Substitute.For<ObservableProperty<string>>("p2");
            //factory.Bind(view, property, (field) => field + "wrap");
            factory.Bind(view, property1, property2, (i1, i2) => i1 + i2);
            Assert.IsTrue(view.Name == "p1p2");
            property1.Value = "p3";
            Assert.IsTrue(view.Name == "p3p2");
            property2.Value = "p4";
            Assert.IsTrue(view.Name == "p3p4");
        }

        [Test]
        public void BindProperty多个字段wrap()
        {
            TestView view = new TestView();
            BindFactory factory = new BindFactory(view);
            ObservableProperty<int> property1 = Substitute.For<ObservableProperty<int>>(1);
            ObservableProperty<int> property2 = Substitute.For<ObservableProperty<int>>(2);
            //factory.Bind(view, property, (field) => field + "wrap");
            factory.Bind(view, property1, property2, (i1, i2) => (i1 + i2) + "wrap");
            Assert.IsTrue(view.Name == "3wrap");
            property1.Value = 2;
            Assert.IsTrue(view.Name == "4wrap");
            property2.Value = 3;
            Assert.IsTrue(view.Name == "5wrap");
        }

        [Test]
        public void 反向绑定()
        {
            TestView view = new TestView();
            BindFactory factory = new BindFactory(view);
            ObservableProperty<int> property = Substitute.For<ObservableProperty<int>>(1);
            factory.RevertBind(view, property);
            ((IComponentEvent<int>) view).GetComponentEvent().Invoke(2);
            Assert.IsTrue(property.Value == 2);
        }

        [Test]
        public void 双向绑定()
        {
            TestView view = new TestView();
            BindFactory factory = new BindFactory(view);
            ObservableProperty<int> property = Substitute.For<ObservableProperty<int>>(1);
            factory.TwoWayBind(view, property);
            Assert.IsTrue(view.Age == 1);
            property.Value = 2;
            Assert.IsTrue(view.Age == 2);
            ((IComponentEvent<int>) view).GetComponentEvent().Invoke(3);
            Assert.IsTrue(property.Value == 3);
        }

        [Test]
        public void 不同双向绑定()
        {
            TestView view = new TestView();
            BindFactory factory = new BindFactory(view);
            ObservableProperty<string> property = Substitute.For<ObservableProperty<string>>("1");
            factory.TwoWayBind<TestView, string, int>(view, property, (i) => i.ToString(), int.Parse);
            Assert.IsTrue(view.Age == 1);
            property.Value = "2";
            Assert.IsTrue(view.Age == 2);
            ((IComponentEvent<int>) view).GetComponentEvent().Invoke(3);
            Assert.IsTrue(property.Value == "3");
        }
    }
}