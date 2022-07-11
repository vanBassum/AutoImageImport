﻿using ImageImporter.Data;
using ImageImporter.Models.Enums;
using SixLabors.ImageSharp;

namespace ImageImporter.Services
{
    public class Settings : SettingsService
    {
        public Settings(ApplicationDbContext context) : base(context)
        {
        }

        public string ImageImportFolder { get => GetPar("wwwroot/mount/import/"); set => SetPar(value); }
        public string ImageExportFolder { get => GetPar("wwwroot/mount/album/"); set => SetPar(value); }
        public string ImageRecycleFolder { get => GetPar("wwwroot/mount/recycle/"); set => SetPar(value); }
        public Size ImageThumbnailSize { get => GetPar(new Size(128, 128)); set => SetPar(value); }
        public bool ImageUseRecycleFolder { get => GetPar(true); set => SetPar(value); }
        public ImageQualityCompareMethods ImageQualityCompareMethod { get => GetPar(ImageQualityCompareMethods.Resolution); set => SetPar(value); }
        public ImageHashingAlgorithms ImageHashingAlgorithm { get => GetPar(ImageHashingAlgorithms.PHashing); set => SetPar(value); }
    }





}
