using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprimo.PublicLinks.Export.CustomObjects
{
    public class AprimoPublicLinkRecord
    {
        public string id { get; set; }
        public object fields { get; set; }
        public object files { get; set; }
        public object preview { get; set; }
        public object thumbnail { get; set; }
        public object masterFile { get; set; }
        public MasterFileLatestVersion masterFileLatestVersion { get; set; }
        public object classifications { get; set; }
        public object accessLists { get; set; }
        public string status { get; set; }
        public string contentType { get; set; }
        public object title { get; set; }
        public object tag { get; set; }
        public object permissions { get; set; }
        public object locks { get; set; }
        public DateTime modifiedOn { get; set; }
        public object modifiedBy { get; set; }
        public DateTime createdOn { get; set; }
        public object createdBy { get; set; }
    }

    public class PublicUri
        {
            public string fileName { get; set; }
            public string renditionName { get; set; }
            public string uri { get; set; }
            public string status { get; set; }
            public string provider { get; set; }
            public bool canDelete { get; set; }
            public object targetType { get; set; }
            public string targetId { get; set; }
            public string fileVersionId { get; set; }
            public string renditionId { get; set; }
            public string renditionType { get; set; }
            public string id { get; set; }
            public string recordId { get; set; }
            public DateTime modifiedOn { get; set; }
            public object modifiedBy { get; set; }
            public DateTime createdOn { get; set; }
            public object createdBy { get; set; }
        }

    public class MasterFileLatestVersion
    {
        public string id { get; set; }
        public string versionLabel { get; set; }
        public int versionNumber { get; set; }
        public string fileName { get; set; }
        public DateTime fileCreatedOn { get; set; }
        public DateTime fileModifiedOn { get; set; }
        public object fileType { get; set; }
        public object filePreviews { get; set; }
        public object masterFilePreview { get; set; }
        public object preview { get; set; }
        public object thumbnail { get; set; }
        public object additionalFiles { get; set; }
        public object renditions { get; set; }
        public object publicLinks { get; set; }
        public object metadata { get; set; }
        public long fileSize { get; set; }
        public string fileExtension { get; set; }
        public string comment { get; set; }
        public int crc32 { get; set; }
        public object content { get; set; }
        public object tag { get; set; }
        public bool preventDownload { get; set; }
        public bool isLatest { get; set; }
        public object duplicateInfo { get; set; }
        public object publicationItems { get; set; }
        public object publications { get; set; }
        public string watermarkId { get; set; }
        public string watermarkType { get; set; }
        public string fileState { get; set; }
        public object watermark { get; set; }
        public PublicUris publicUris { get; set; }
        public object permissions { get; set; }
        public object usedIn { get; set; }
        public object contains { get; set; }
        public DateTime createdOn { get; set; }
        public object createdBy { get; set; }
    }

    public class PublicUris
        {
            public List<PublicUri> items { get; set; }
        }

        

    
}
