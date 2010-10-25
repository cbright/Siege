﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Siege.Courier.Web
{
    public class TypeFinder
    {
        private string name;
        private string @namespace;
        private Type baseType;

        public TypeFinder Named(string name)
        {
            this.name = name;
            return this;
        }

        public TypeFinder InNamespace(string @namespace)
        {
            this.@namespace = @namespace;
            return this;
        }

        public TypeFinder Implementing<TBaseType>()
        {
            this.baseType = typeof(TBaseType);
            return this;
        }

        public Type Find()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = assemblies.SelectMany(a => a.GetTypes());

            if(!string.IsNullOrEmpty(this.@namespace))
            {
                types = types.Where(t => t.Namespace == this.@namespace);
            }

            if(this.baseType != null)
            {
                types = types.Where(t => this.baseType.IsAssignableFrom(t));
            }

            if (!string.IsNullOrEmpty(this.name))
            {
                types = types.Where(t => t.Name.ToLower() == this.name.ToLower());
            }

            return types.FirstOrDefault();
        }
    }
}