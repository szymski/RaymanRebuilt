﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RREngine.Engine.Resources
{
    /// <summary>
    /// Reference counting unmanaged resource manager.
    /// Every resource should be an instance of <seealso cref="Resource"/>.
    /// </summary>
    public class ResourceManager
    {
        private Dictionary<Resource, int> _resources = new Dictionary<Resource, int>();

        private bool _freeingAll = false;

        /// <summary>
        /// Registers a resource with initial reference count of 1.
        /// </summary>
        public void RegisterResource(Resource resource)
        {
            Engine.Logger.Log(new[] { "resource" }, $"Registering {resource.GetType()} resource");
            _resources.Add(resource, 1);
        }

        /// <summary>
        /// Increments reference count by one.
        /// </summary>
        public void IncrementReferenceCount(Resource resource)
        {
            _resources[resource]++;
        }

        /// <summary>
        /// Decrements reference count by one.
        /// If no references afterwards, destroys the resource.
        /// </summary>
        public void DecrementReferenceCount(Resource resource)
        {
            if (_freeingAll)
                return;

            _resources[resource]--;

            if (_resources[resource] == 0)
            {
                _resources.Remove(resource);

                Engine.Logger.Log(new[] { "resource" }, $"Destroying {resource.GetType()} resource");
                resource.Destroy();
            }
            else if (_resources[resource] < 0)
            {
                Engine.Logger.LogWarning(new[] { "resource" }, $"Resource {resource.GetType()} reference count below 0");
            }
        }

        public void FreeAllResources()
        {
            Engine.Logger.Log(new[] { "resource" }, $"Destroying all resources");

            _freeingAll = true;

            foreach (var resource in _resources)
                resource.Key.Destroy();

            _resources.Clear();

            _freeingAll = false;
        }
    }
}
