using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AWSApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AWSApplication.Controller
{
    [Route("api/AWSS3Bucket")]
    [ApiController]
    public class AWSS3BucketController : ControllerBase
    {
        private readonly IS3Service _s3service;
        public AWSS3BucketController(IS3Service s3service)
        {
            this._s3service = s3service;
        }
        [HttpPost("{BucketName}")]
        public async Task<IActionResult> CreateBucket([FromRoute] string BucketName)
        {
            var response = await _s3service.CreateBucketAsync(BucketName);
            return Ok(response);
        }

        [HttpPost("{BucketName}")]
        [Route("AddFileToBucket")]
        public async Task<IActionResult> AddFileToBucket([FromRoute] string BucketName)
        {
            await _s3service.UploadFileAsync(BucketName);
            return Ok();
        }

        [HttpPost("{BucketName}")]
        [Route("DeleteFileFromBucket")]
        public async Task<IActionResult> DeleteFileFromBucket([FromRoute] string BucketName)
        {
            await _s3service.DeleteObjectNonVersionedBucketAsync(BucketName);
            return Ok();
        }

    }
}
