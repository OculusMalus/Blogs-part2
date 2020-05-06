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
                    Console.WriteLine("1) Display all blogs?");
                    Console.WriteLine("2) Add new blog?");
                    Console.WriteLine("3) Create blog post?");
                    Console.WriteLine("Enter any other key to exit.");

                    choice = Console.ReadLine();


                    if (choice == "1")
                    {
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
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

                        var blog = new Blog { Name = name };

                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }

                    if (choice == "3")
                    {
                        //select the blog in which to post
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);
                        int selectedBlogId = 0;

                        Console.WriteLine("All blogs in the database:");
                        Console.WriteLine("Blog ID\t\tBlog Name");
                        foreach (var item in query)
                        {
                            Console.WriteLine("{0}\t\t{1}", item.BlogId, item.Name);
                        }
                        try
                        {
                            Console.Write("Enter the Blog ID of the blog to which you wish to post: ");
                            selectedBlogId = int.Parse(Console.ReadLine());
                        }
                        catch (Exception ex)
                        { logger.Error(ex.Message); }

                        //Console.Write("Enter the Blog ID of the blog to which you wish to post: ");
                        //selectedBlogId = IntParse().Console.ReadLine();
                        //Add new blog post
                        Console.Write("Enter blog post title: ");
                        var title = Console.ReadLine();

                        Console.Write("Enter your blog content: ");
                        var content = Console.ReadLine();

                        var post = new Post { Title = title, Content = content, BlogId = selectedBlogId };

                        db.AddPost(post);
                        logger.Info("Post added - {title}", title);

                    }

                } while (choice == "1" || choice == "2" || choice == "3");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}