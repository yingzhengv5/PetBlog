import axios from "axios";
import { Post } from "../Models/Post";

const api = axios.create({
  baseURL: "/api",
  // baseURL: import.meta.env.VITE_API_URL || "http://localhost:5214/api",
});

// Get all posts
export const getPosts = async () => {
  return await api.get<Post[]>("/petpost");
};

// Get post by ID
export const getPostById = async (id: number) => {
  return await api.get<Post>(`/petpost/${id}`);
};

// Add a new post
export const addPost = async (postData: FormData) => {
  return await api.post<Post>("/petpost", postData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
};

// Update a post
export const updatePost = async (id: number, postData: FormData) => {
  return await api.put<Post>(`/petpost/${id}`, postData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
};

// delete a post
export const deletePost = async (id: number) => {
  return await api.delete(`/petpost/${id}`);
};