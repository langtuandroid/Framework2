﻿using System;
using UnityEngine;

namespace Framework
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TransformPath : Attribute
    {
        public string Path;

        public TransformPath(string path)
        {
            this.Path = path;
        }
    }
}