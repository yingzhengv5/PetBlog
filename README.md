Welcome to the PetBlog web app! Here, you can explore interesting stories about pets with their images, edit and delete posts, and view all posts.

## Running the Project

Backend :

1. Navigate to the project directory:
   `cd backend`

2. Restore dependencies:
   `dotnet Restore`

3. Update environment:<br>
   Change .envExample file name to .env, update the required information.

4. In Program.cs, change MySqlServerVersion to your own version, change CORS to your own frontend IP address.

5. Run the project:
   `dotnet run`

Frontend :

1. Navigate to the project directory:
   `cd frontend`

2. Install dependencies:
   `npm install`

3. Run Project:
   `npm run start`

## I'm proud of :

The user can upload and delete images. A default image will be used if the user chooses not to upload one. All the images are stored in Cloudinary. When the delete action is executed, the image will also be removed from Cloudinary.

## Basic Features:

1. Basic CRUD operations
2. Frontend using React with TypeScript
3. MUI library for UI display
4. Responsive web app
5. Use of React Router
6. ASP.NET with C# for backend
7. Use of EF Core
8. MySQL database for local development
9. Regular commits

## Advanced Features:

1. Toggle light/dark theme
2. Containerize project using docker
3. Unit test
