using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;

namespace _301141338_Mbugua__LabThree
{
    public class AWS_Driver
    {
        private const string awsAccessKey = "";
        private const string awsSecretKey = "";
        private const string bucketName = "centennialbucket";
        public IAmazonS3 s3_Client;
        public AmazonDynamoDBClient dynamoDBClient;
        public static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;

        public AWS_Driver()
        {
            dynamoConnection();
            s3Connection();
        }

        public void dynamoConnection()
        {
            try
            {
                dynamoDBClient = new AmazonDynamoDBClient(awsAccessKey, awsSecretKey, RegionEndpoint.USEast2);
            }
            catch (AmazonDynamoDBException ex)
            {
                /*PopUp.createPopUp(ex.Message);*/
                Console.WriteLine(ex.Message);
            }
        }

        public void s3Connection()
        {
            try
            {
                s3_Client = new AmazonS3Client(awsAccessKey, awsSecretKey, RegionEndpoint.USEast1);
            }
            catch (AmazonS3Exception ex)
            {
                /*PopUp.createPopUp(ex.Message);*/
                Console.WriteLine(ex.Message);
            }
        }
        
        public async void pullMovie(string filename)
        {
            AWS_Driver driver = new AWS_Driver();

            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = "centennialbucket";
            filename += ".mp4";
            request.Key = filename;

            try
            {
                using GetObjectResponse response = await driver.s3_Client.GetObjectAsync(request);
                await response.WriteResponseStreamToFileAsync($"\\Users\\" + Environment.UserName + "\\Downloads\\" + filename, true, CancellationToken.None);

            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void putMovie(string filename, string movieTitle, string movieDirector, string genre)
        {
            
            AWS_Driver driver = new AWS_Driver();
            try
            {
                await driver.s3_Client.UploadObjectFromFilePathAsync("centennialbucket", filename, "./" + filename, null);
            } catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Dictionary<String, AttributeValue> newItem = new Dictionary<string, AttributeValue>()
            {

                
                { "MovieTitle", new AttributeValue { S = movieTitle} },
                { "MovieDirector", new AttributeValue { S = movieDirector } },
                { "Comments", new AttributeValue { L = new List<AttributeValue>() {
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }  
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            }
                        } 
                    } 
                },
                { "Genre", new AttributeValue { S = genre } },
                { "Ratings", new AttributeValue { L = new List<AttributeValue>() {
                    { new AttributeValue { N = "10"} },
                } } },
                { "Release Time", new AttributeValue { S = DateTime.Now.ToString() } },
                { "Uploader", new AttributeValue { S = UserAccountDriver.username } }
            };

            try {
                await driver.dynamoDBClient.PutItemAsync("Videos", newItem);
            } catch (AmazonDynamoDBException ex) {
                Console.WriteLine(ex.Message);
            }
        }
            
        public async void deleteMovie(string movieName)
        {
            /*filename += ".mp4";*/
            AWS_Driver driver = new AWS_Driver();

            Movie movie = Movie.findMovie(movieName);
            try
            {
                Dictionary<String, AttributeValue> oldItem = new Dictionary<string, AttributeValue>()
                {
                    { "MovieTitle", new AttributeValue { S = movie.movieTitle} },
                    { "MovieDirector", new AttributeValue { S = movie.movieDirector } }
                };

                DeleteObjectRequest deleteObject = new DeleteObjectRequest()
                {
                    Key = movieName,
                    BucketName = "centennialbucket"
                };

                await driver.dynamoDBClient.DeleteItemAsync("Videos", oldItem);
                await driver.s3_Client.DeleteObjectAsync(deleteObject);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void updateMovie(Movie movie, string movieDirector, string genre, string releaseTime)
        {

            AWS_Driver driver = new AWS_Driver();

            Dictionary<String, AttributeValue> oldItem = new Dictionary<string, AttributeValue>()
            {
                { "MovieTitle", new AttributeValue { S = movie.movieTitle} },
                { "MovieDirector", new AttributeValue { S = movie.movieDirector } }
            };

            Dictionary<String, AttributeValue> newItem = new Dictionary<string, AttributeValue>()
            {
                { "MovieTitle", new AttributeValue { S = movie.movieTitle} },
                { "MovieDirector", new AttributeValue { S = movieDirector } },
                { "Comments", new AttributeValue { L = movie.comments } },
                { "Genre", new AttributeValue { S = genre } },
                { "Ratings", new AttributeValue { L = movie.ratings } },
                { "Release Time", new AttributeValue { S = movie.releaseTime } },
                { "Uploader", new AttributeValue { S = UserAccountDriver.username } }
            };

            try
            {
                await driver.dynamoDBClient.DeleteItemAsync("Videos", oldItem);
                await driver.dynamoDBClient.PutItemAsync("Videos", newItem);
            }
            catch (AmazonDynamoDBException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async void updateMovie(string movieName)
        {

            AWS_Driver driver = new AWS_Driver();

            Movie movie = Movie.findMovie(movieName);

            Dictionary<String, AttributeValue> oldItem = new Dictionary<string, AttributeValue>()
            {
                { "MovieTitle", new AttributeValue { S = movie.movieTitle} },
                { "MovieDirector", new AttributeValue { S = movie.movieDirector } }
            };

            Dictionary<String, AttributeValue> newItem = new Dictionary<string, AttributeValue>()
            {
                { "MovieTitle", new AttributeValue { S = movie.movieTitle} },
                { "MovieDirector", new AttributeValue { S = movie.movieDirector } },
                { "Comments", new AttributeValue { L = movie.comments } },
                { "Genre", new AttributeValue { S = movie.genre } },
                { "Ratings", new AttributeValue { L = movie.ratings } },
                { "Release Time", new AttributeValue { S = movie.releaseTime } },
                { "Uploader", new AttributeValue { S = movie.uploader } }
            };

            try
            {
                await driver.dynamoDBClient.DeleteItemAsync("Videos", oldItem);
                Thread.Sleep(1000);
                await driver.dynamoDBClient.PutItemAsync("Videos", newItem);
            }
            catch (AmazonDynamoDBException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        }
}

/*List<AttributeValue> comments = new List<AttributeValue>() {
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } },
                                        { new AttributeValue { S = "blank" } }
                                    }
                                }
                            }
            };*/