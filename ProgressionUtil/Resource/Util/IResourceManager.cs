﻿namespace Progression.Resource.Util
{
    public interface IResourceManager
    {
        /// <summary>
        /// This method is only to be called by the IResourceable itself. It will fail unless IsFrozen returns true
        /// </summary>
        void FreezeResourceable(IResourceable resourceable);
    }
}