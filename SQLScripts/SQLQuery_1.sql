CREATE DATABASE DotNetCourseDatabase;

USE DotNetCourseDatabase;

SELECT TABLE_NAME 
FROM CheriesBlog.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'

SELECT * FROM __EFMigrationsHistory

-- DROP TABLE __EFMigrationsHistory
-- DROP TABLE CheriesBlog.Comments
-- DROP TABLE CheriesBlog.Posts
-- DROP TABLE CheriesBlog.AspNetRoleClaims
-- DROP TABLE CheriesBlog.AspNetRoles
-- DROP TABLE CheriesBlog.AspNetUserClaims
-- DROP TABLE CheriesBlog.AspNetUserLogins
-- DROP TABLE CheriesBlog.AspNetUserRoles
-- DROP TABLE CheriesBlog.AspNetUsers
-- DROP TABLE CheriesBlog.AspNetUserTokens


CREATE SCHEMA CheriesBlog;

CREATE TABLE CheriesBlog.user_pool (
    id INT IDENTITY(1, 1) PRIMARY KEY,
    email NVARCHAR(100),
    password NVARCHAR(250),
    name NVARCHAR(20)
);

SELECT * FROM CheriesBlog.AspNetUsers;

CREATE TABLE CheriesBlog.blog_posts (
    id INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(250),
    subtitle NVARCHAR(250),
    author_id INT FOREIGN KEY REFERENCES cherieBlog.user_pool(id),
    date NVARCHAR(250),
    body NVARCHAR(MAX),
    img_url  NVARCHAR(250)
)

SELECT * FROM CheriesBlog.Posts;

UPDATE CheriesBlog.Posts SET Posts.CreatedAt = '2025-04-10' FROM CheriesBlog.Posts AS Posts WHERE Posts.Id = 1;
UPDATE CheriesBlog.Posts SET Posts.ImageUrl = 'https://unsplash.com/photos/pink-flower-petals-on-white-background-95QNbCkVERM' FROM CheriesBlog.Posts AS Posts WHERE Posts.Id = 2;
-- UPDATE CheriesBlog.Comments SET Comme



-- 6e72c2b4-6aee-487a-9a4b-1e0c8aed85cd

CREATE TABLE CheriesBlog.comments (
    id INT IDENTITY(1,1) PRIMARY KEY,
    text NVARCHAR(MAX),
    post_id INT FOREIGN KEY REFERENCES CheriesBlog.blog_posts(id),
    commenter_id INT FOREIGN KEY REFERENCES CheriesBlog.user_pool(id)
);

SELECT * FROM CheriesBlog.comments;

UPDATE CheriesBlog.Comments SET Content = 'Content 1' FROM CheriesBlog.Comments AS Comments WHERE Comments.Id = 1

INSERT INTO CheriesBlog.Posts (email, password, name)
VALUES ('chane.c.sun@gmail.com', 'password', 'Cherie')

INSERT INTO CheriesBlog.comments (text, post_id, commenter_id)
VALUES ('TEST Comment', 1, 1)

INSERT INTO CheriesBlog.blog_posts (title, subtitle, author_id, date, body, img_url)
VALUES ('TEST title', 'TEST Subtitle', 1, 'Mar 25, 2025', 'Test Body', 'https://plus.unsplash.com/premium_photo-1683121825174-ff1620a5e387?q=80&w=1925&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D')


