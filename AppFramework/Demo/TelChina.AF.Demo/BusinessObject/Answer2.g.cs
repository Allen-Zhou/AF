﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using TelChina.AF.Persistant;

namespace TelChina.AF.Demo
{
    public partial class Answer2 : EntityBase
    {
        private String _name;

        private Guid? _question_ID;

        public virtual String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != value)
                {
                    RaisPropertyChangIngEvent("Name");
                    _name = value;
                    RaisPropertyChangedEvent("Name");
                }
            }
        }

        public virtual Guid? Question_ID
        {
            get
            {
                return this._question_ID;
            }
            set
            {
                if (this._question_ID != value)
                {
                    RaisPropertyChangIngEvent("Question_ID");
                    _question_ID = value;
                    RaisPropertyChangedEvent("Question_ID");
                }
            }
        }
        public override string EntityComponent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private Question2 _question;

        public virtual Question2 Question
        {
            get { return _question; }
            set { _question = value; }
        }
    }
}
