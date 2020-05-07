using Blogs_part2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs_part2
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string choice = "";
                var db = new BloggingContext();
                do
                {


                    //Present user with options
                    Console.WriteLine("\n1) Display all blogs");
                    Console.WriteLine("2) Add new blog");
                    Console.WriteLine("3) Create blog post");
                    Console.WriteLine("4) Display blog posts");
                    Console.WriteLine("Enter any other key to exit.");

                    choice = Console.ReadLine();
                    logger.Info("Option {0} selected", choice);

                    if (choice == "1")
                    {
                        
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("{0} blogs returned", query.Count());
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                        
                    }

                    if (choice == "2")
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        if (name.Length > 0)
                        {
                            var blog = new Blog { Name = name };
                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                        }
                        else logger.Error("Blog name must not be empty.");
                    }

                    if (choice == "3")
                    {
                        //select the blog in which to post
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        int selectedBlogId = 0;

                        //Console.WriteLine("All blogs in the database:");
                        //Console.WriteLine("Blog ID\t\tBlog Name");
                        foreach (var item in query)
                        {
                            Console.WriteLine("{0}) {1}", item.BlogId, item.Name);
                        }
                        try
                        {
                            Console.Write("Enter the Blog number to which you wish to post: ");
                            selectedBlogId = int.Parse(Console.ReadLine());
                            if (query.Any(b => b.BlogId == selectedBlogId))
                            {
                                //Add new blog post
                                Console.Write("Enter blog post title: ");
                                var title = Console.ReadLine();
                                if (title.Length > 0)
                                {
                                    Console.Write("Enter your blog content: ");
                                    var content = Console.ReadLine();
                                    var post = new Post { Title = title, Content = content, BlogId = selectedBlogId };

                                    db.AddPost(post);
                                    logger.Info("Post added - {title}", title);
                                }
                                else logger.Error("Post title must not be empty.");
                            } else logger.Error("No blog with that ID exists.");

                        }
                        catch (Exception ex)
                        { 
                            logger.Error(ex.Message); 
                        }
                    }

                    if (choice == "4")
                    {
                        Console.Clear();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        int selection = 0;
                        //Present user with options
                        Console.WriteLine("Select blog posts to display:");
                        Console.WriteLine("0) All blog posts");                        
                        foreach (var item in query)
                        {
                            Console.WriteLine("{0}) Posts from {1}", item.BlogId, item.Name);
                        }

                        try {
                            selection = int.Parse(Console.ReadLine());
                            if (selection != 0)
                            {
                                if (query.Any(b => b.BlogId == selection))
                                {
                                    //display individual blog's posts
                                    var posts = db.Posts.Where(p => p.BlogId == selection);

                                    Console.WriteLine("{0} post/s returned", posts.Count());
                                    foreach (var item in posts)
                                    {
                                        Console.WriteLine("Blog: {0}", item.Blog.Name);
                                        Console.WriteLine("Title: {0}", item.Title);
                                        Console.WriteLine("Content: {0}\n", item.Content);
                                    }
                                }
                                else logger.Error("No blog with that ID exists.");

                            }
                            else
                            {
                                //display all posts
                                var posts = db.Posts.OrderBy(p => p.PostId);
                                Console.WriteLine("{0} post/s returned", posts.Count());
                                foreach (var item in posts)
                                {                                  
                                    Console.WriteLine("Blog: {0}", item.Blog.Name);
                                    Console.WriteLine("Title: {0}", item.Title);
                                    Console.WriteLine("Content: {0}\n", item.Content);
                                }                                
                            }
                        }
                        catch (Exception ex) {
                            logger.Error(ex.Message);
                        }               

                    }

                } while (choice == "1" || choice == "2" || choice == "3" || choice == "4");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}