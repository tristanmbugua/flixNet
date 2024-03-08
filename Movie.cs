using Amazon.DynamoDBv2.Model;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace _301141338_Mbugua__LabThree
{
    public class Movie
    {
        static AWS_Driver aws_driver = new AWS_Driver();
        public static List<Dictionary<String, AttributeValue>> movieItems;
        public static List<Movie> movies;
        public String movieTitle;
        public String movieDirector;
        public List<AttributeValue> comments;
        public List<List<string>> unpackedComments;
        public String genre;
        public List<AttributeValue> ratings;
        public String releaseTime;
        public String uploader;
        public double averageRating;
        public static List<SelectListItem> genres = new List<SelectListItem> {
           new SelectListItem{Text = "Comedy", Value = "Comedy"},
           new SelectListItem{Text = "Drama", Value = "Drama"},
            new SelectListItem{Text = "Thriller", Value = "Thriller"},
            new SelectListItem{Text = "Fantasy", Value = "Fantasy"},
            new SelectListItem{Text = "Horror", Value = "Horror"},
            new SelectListItem{Text = "Science Fiction", Value = "Science Fiction"}
        };
        public static List<SelectListItem> movieTitles = new List<SelectListItem>();

        public Movie()
        {
            this.movieTitle = "Error!";
        }
        public Movie(string movieTitle, string movieDirector, List<AttributeValue> comments, string genre, List<AttributeValue> ratings, string releaseTime, string uploader)
        {
            this.movieTitle = movieTitle;
            this.movieDirector = movieDirector;
            this.comments = comments;
            this.genre = genre;
            this.ratings = ratings;
            this.releaseTime = releaseTime;
            this.uploader = uploader;

            calculateRating();
            unpackComments();
        }

        public void calculateRating()
        {
            if (ratings == null)
            {
                averageRating = 0;
            }
            else
            {
                averageRating = 0;
                /*int total = 0;

                for (int i = 0; i < this.ratings.Length; i++)
                {
                    total += this.ratings[i];
                }

                averageRating = (Double)total / (Double)ratings.Length;*/
            }
        }

        public override string ToString()
        {
            return $"Title: {movieTitle} | Director: {movieDirector} | Rating: {Math.Round(averageRating, 2)} | Genre: {genre}";
        }
        
        public static void initializeMovies()
        {
            List<string> key = new List<string>
            {
                "MovieTitle", "MovieDirector", "Comments",
                "Genre", "Ratings", "Release Time", "Uploader"
            };

            movieItems = aws_driver.dynamoDBClient.ScanAsync("Videos", key).Result.Items;

            fillMovies();
        }

        public static void fillMovies()
        {
            movies = new List<Movie>();
            movieTitles = new List<SelectListItem>();

            foreach (var item in movieItems)
            {
                Movie movie = new Movie(
                item.Values.ElementAt(1).S,
                item.Values.ElementAt(0).S,
                item.Values.ElementAt(2).L,
                item.Values.ElementAt(5).S,
                item.Values.ElementAt(6).L,
                item.Values.ElementAt(3).S,
                item.Values.ElementAt(4).S
                );

                movies.Add(movie);
                movieTitles.Add(
                    new SelectListItem { 
                        Text = item.Values.ElementAt(1).S,
                        Value = item.Values.ElementAt(1).S
                    }
                );
            }
        }
        
        public static List<String> readComments(String movieName)
        {
            Movie.initializeMovies();

            Movie movieToRead = Movie.findMovie(movieName);

            List<String> comments = new List<String>();
            
            foreach (AttributeValue val in movieToRead.comments.ToList()) {
                comments.Add(val.S);
            }

            return comments;
        }
    
        public static List<Movie> genertateMovieList()
        {
            
            Movie.initializeMovies();
            return Movie.movies;
        }

        public static List<SelectListItem> generateMovieNames()
        {
            Movie.initializeMovies();

            List<SelectListItem> movieList = new List<SelectListItem>();

            for (int i = 0; i < Movie.movies.Count; i++)
            {
                movieList.Add(new SelectListItem { Text = Movie.movies.ElementAt(i).movieTitle, Value = Movie.movies.ElementAt(i).movieTitle });
            }

            return movieList;
        }

        public static Movie findMovie(string movieName)
        {
            foreach (Movie movie in Movie.movies)
            {
                if (movie.movieTitle.Equals(movieName))
                    return movie;
            }
            return null;
        }
    
        public void unpackComments()
        {
            List<List<String>> nestedStrings = new List<List<string>>();

            for (int i = 0; i < comments.Count; i++)
            {
                AttributeValue val = comments[i];
                List<String> strings = new List<String>();
                foreach (AttributeValue val_ in val.L.ToList())
                {
                    strings.Add(val_.S);
                }
                nestedStrings.Add(strings);
            }
            this.unpackedComments = nestedStrings;
        }
    
        public void repackComments()
        {
            List<AttributeValue> nestedComments = new List<AttributeValue>();
                foreach (List<string> val in unpackedComments)
                {
                    nestedComments.Add(new AttributeValue
                        {
                            L = new List<AttributeValue>() {
                                { new AttributeValue { S = val.ElementAt(0)} },
                                { new AttributeValue { S = val.ElementAt(1)} },
                                { new AttributeValue { S = val.ElementAt(2)} }
                            }
                        }
                    );
                }
            this.comments = nestedComments;
        }
        
        public static List<Movie> searchByGenre(string genre)
        {
            List<Movie> returns = new List<Movie>();
            foreach (Movie movie in Movie.movies)
            {
                if ((movie.genre.Equals(genre)))
                {
                    returns.Add(movie);
                }
            }
            return returns;
        }
        
        public static List<Movie> searchByRating(int min, int max)
        {
            List<Movie> returns = new List<Movie>();
            foreach(Movie movie in Movie.movies) {
                if ((movie.averageRating >= min) && (movie.averageRating <= max))
                {
                    returns.Add(movie);
                }
            }
            return returns;
        }
        
        public void editComments(
                string comment0, string author0, string time0,
                string comment1, string author1, string time1,
                string comment2, string author2, string time2,
                string comment3, string author3, string time3,
                string comment4, string author4, string time4
                )
        {
            List<AttributeValue> comments = new List<AttributeValue>() {
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = comment0 } },
                                        { new AttributeValue { S = author0 } },
                                        { new AttributeValue { S = time0 } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = comment1 } },
                                        { new AttributeValue { S = author1 } },
                                        { new AttributeValue { S = time1 } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = comment2 } },
                                        { new AttributeValue { S = author2 } },
                                        { new AttributeValue { S = time2 } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = comment3 } },
                                        { new AttributeValue { S = author3 } },
                                        { new AttributeValue { S = time3 } }
                                    }
                                }
                            },
                            { new AttributeValue { L = new List<AttributeValue>() {
                                        { new AttributeValue { S = comment4 } },
                                        { new AttributeValue { S = author4 } },
                                        { new AttributeValue { S = time4 } }
                                    }
                                }
                            }
            };

            this.comments = this.compareComments(comments);
            Thread.Sleep(100);
            aws_driver.updateMovie(this.movieTitle);
        }

        public List<AttributeValue> compareComments(List<AttributeValue> newComments)
        {
            for(int i = 0; i < this.comments.Count; i++) {
                if (!newComments.ElementAt(i).L.ElementAt(0).S.Equals(this.comments.ElementAt(i).L.ElementAt(0).S)) {
                    newComments.ElementAt(i).L.ElementAt(1).S = UserAccountDriver.username;
                    newComments.ElementAt(i).L.ElementAt(2).S = DateTime.Now.ToString();
                }
            }
            return newComments;
        }
        
        public void addComment(string comment, string author, string time)
        {
            List<AttributeValue> comments = new List<AttributeValue> {
                    new AttributeValue { S = comment},
                    new AttributeValue { S = author},
                    new AttributeValue { S = time}
                };

            restructureComments(comments);
            Thread.Sleep(100);
            aws_driver.updateMovie(this.movieTitle);
        }

        public void restructureComments(List<AttributeValue> comment)
        {
            this.comments.ElementAt(0).L = this.comments.ElementAt(1).L.ToList();
            this.comments.ElementAt(1).L = this.comments.ElementAt(2).L.ToList();
            this.comments.ElementAt(2).L = this.comments.ElementAt(3).L.ToList();
            this.comments.ElementAt(3).L = this.comments.ElementAt(4).L.ToList();
            this.comments.ElementAt(4).L = comment;
        }
    
        public void addRating(int rating)
        {
            this.ratings.Add(new AttributeValue { N = rating.ToString() });
            Thread.Sleep(1000);
            aws_driver.updateMovie(this.movieTitle);
        }
    }
}