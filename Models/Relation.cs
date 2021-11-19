using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PracticeAPI.Models
{
    // PERSON DATA

    public class Person
    {
        public string Cedula { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Profesion { get; set; }
    }

    // PARENT DATA

    public interface iParent : iEntity
    {
        ICollection<Child> Childs { get; set; }
    }
    public class Parent : Person, iParent, iEntity
    {
        public Guid Id { get; set; }
        public ICollection<Child> Childs { get; set; }
    }

    public class ParentA : Parent, iParent, iEntity
    {
        public new ICollection<ChildA> Childs { get; set; }
    }
    public class ParentB : Parent, iParent, iEntity
    {
        public new ICollection<ChildB> Childs { get; set; }
    }

    // CHILD DATA

    public interface iChild : iEntity
    {
        ICollection<Thing> Things { get; set; }
    }
    public class Child : Person, iChild, iEntity
    {
        public Guid Id { get; set; }
        public ICollection<Thing> Things { get; set; }
    }
    public class ChildA : Child, iChild, iEntity
    {
        public new ICollection<ThingA> Things { get; set; }
    }
    public class ChildB : Child, iChild, iEntity
    {
        public new ICollection<ThingB> Things { get; set; }
    }

    // THING DATA

    public interface iThing : iEntity {}
    public class Thing : iThing, iEntity
    {
        public Guid Id { get; set; }
        public string Word { get; set; }
        public int Num { get; set; }
    }
    public class ThingA : Thing, iThing, iEntity {}
    public class ThingB : Thing, iThing, iEntity {}

}