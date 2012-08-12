﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Squiggle.Activity;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using Squiggle.Core.Chat;
using Squiggle.UI.MessageFilters;
using Squiggle.UI.MessageParsers;
using Squiggle.Plugins.MessageFilter;
using Squiggle.Plugins.MessageParser;
using Squiggle.Core.Chat.Activity;

namespace Squiggle.UI.Components
{
    class PluginLoader
    {
        [ImportMany(typeof(IActivity))]
        public IEnumerable<IActivity> Activities { get; set; }

        [ImportMany(typeof(IMessageFilter))]
        public IEnumerable<IMessageFilter> MessageFilters { get; set; }

        [ImportMany(typeof(IMessageParser))]
        public IEnumerable<IMessageParser> MessageParsers { get; set; }

        public PluginLoader(ComposablePartCatalog catalog)
        {
            var container = new CompositionContainer(catalog);
            container.SatisfyImportsOnce(this);
        }

        public bool HasActivity(Guid id)
        {
            return GetActivity(id) != null;
        }

        public IActivityHandler GetActivityHandler(Guid activityId, Func<IActivity, IActivityHandler> getAction)
        {
            IActivity factory = GetActivity(activityId);
            if (factory == null)
                return null;
            IActivityHandler handler = getAction(factory);
            return handler;
        }

        IActivity GetActivity(Guid activityId)
        {
            IActivity activity = Activities.FirstOrDefault(f => f.Id.Equals(activityId));
            return activity;
        }
    }
}