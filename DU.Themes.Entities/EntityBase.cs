﻿using System;

namespace DU.Themes.Entities
{
    /// <summary>
    /// Base Entity class
    /// </summary>
    public abstract class EntityBase : IDentifiable
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public long Id { get; set; }

        public DateTime TouchTime { get; set; }
    }
}
