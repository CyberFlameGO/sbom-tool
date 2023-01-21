﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.Sbom.Common.Config;
using Microsoft.Sbom.Common;

namespace Microsoft.Sbom.Api.Filters
{
    public class ManifestFolderFilter : IFilter<ManifestFolderFilter>
    {
        private readonly IConfiguration configuration;
        private readonly IFileSystemUtils fileSystemUtils;
        private readonly IOSUtils osUtils;
        private readonly IContext context;
        private string manifestFolderPath;

        public ManifestFolderFilter(
            IConfiguration configuration,
            IFileSystemUtils fileSystemUtils,
            IOSUtils osUtils,
            IContext context)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.fileSystemUtils = fileSystemUtils ?? throw new ArgumentNullException(nameof(fileSystemUtils));
            this.osUtils = osUtils ?? throw new ArgumentNullException(nameof(osUtils));
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            Init();
        }

        public bool IsValid(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            var normalizedPath = new FileInfo(filePath).FullName;

            return !normalizedPath.StartsWith(manifestFolderPath, osUtils.GetFileSystemStringComparisonType());
        }

        public void Init()
        {
            manifestFolderPath = new FileInfo(context.ManifestDirPath.Value).FullName;
        }
    }
}
