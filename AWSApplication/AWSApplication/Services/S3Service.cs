using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using AWSApplication.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AWSApplication.Services
{
    public class S3Service :IS3Service
    {
        private static IAmazonS3 _client;
        private const string keyName = "testKeyName";
        private const string filePath = "C:\\Users\\ASUS\\Desktop\\testdata.txt";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;
        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }

        public async Task<S3Response> CreateBucketAsync(string name)
        {
            try
            {
                S3Response Response = null;
                if(await AmazonS3Util.DoesS3BucketExistV2Async(_client,name)==false)
                {
                    var bucketrequest = new PutBucketRequest
                        {
                            BucketName=name,
                            UseClientRegion=true
                        };
                    var bucketresponse = await _client.PutBucketAsync(bucketrequest);
                    Response=new S3Response { Message = bucketresponse.ResponseMetadata.RequestId, status = bucketresponse.HttpStatusCode };
                }
                return Response;
            }
            catch(AmazonS3Exception e1)
            {
                return new S3Response { Message = e1.Message, status = e1.StatusCode };
                //Console.WriteLine(e1.StatusCode+"--"+e1.Message);
            }
            catch (Exception e1)
            {
                return new S3Response { Message = e1.Message, status = HttpStatusCode.InternalServerError };
                //Console.WriteLine(e1.StatusCode+"--"+e1.Message);
            }
        }

        
        public async Task UploadFileAsync(string bucketName)
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(_client);

                // Option 1. Upload a file. The file name is used as the object key name.
                await fileTransferUtility.UploadAsync(filePath, bucketName);
                Console.WriteLine("Upload 1 completed");

                // Option 2. Specify object key name explicitly.
                await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                Console.WriteLine("Upload 2 completed");

                // Option 3. Upload data from a type of System.IO.Stream.
                using (var fileToUpload =
                    new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload,
                                               bucketName, keyName);
                }
                Console.WriteLine("Upload 3 completed");

                // Option 4. Specify advanced settings.
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    FilePath = filePath,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB.
                    Key = keyName,
                    CannedACL = S3CannedACL.Private
                };
                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                Console.WriteLine("Upload 4 completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
        public async Task DeleteObjectNonVersionedBucketAsync(string bucketName)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                Console.WriteLine("Deleting an object");
                await _client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
        }


    }
}
