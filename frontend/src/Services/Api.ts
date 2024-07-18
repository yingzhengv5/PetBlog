import axios from "axios";
import { Post } from "../Models/Post";

const api = axios.create({
  baseURL: "http://localhost:5214/api",
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
export const addPost = async (post: Omit<Post, "id" | "createAt">) => {
  return await api.post<Post>("/petpost", post);
};

// Update a post
export const updatePost = async (id: number, post: Omit<Post, "createAt">) => {
  return await api.put(`/petpost/${id}`, post);
};

// delete a post
export const deletePost = async (id: number) => {
  return await api.delete(`/petpost/${id}`);
};
