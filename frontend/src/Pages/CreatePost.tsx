import React, { useState } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Card,
  CardContent,
  CardActions,
} from "@mui/material";
import { addPost } from "../Services/Api";
import { useNavigate } from "react-router-dom";

const CreatePost: React.FC = () => {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [imageUrl, setImageUrl] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    try {
      const newPost = await addPost({ title, content, imageUrl });
      navigate(`/posts/${newPost.data.id}`);
    } catch (error) {
      console.error("Failed to create post:", error);
      alert("Failed to create post. Please try again.");
    }
  };

  return (
    <Container maxWidth="sm" sx={{ marginTop: 4 }}>
      <Card>
        <CardContent>
          <Typography variant="h4" gutterBottom>
            Create New Post
          </Typography>
          <form onSubmit={handleSubmit}>
            <TextField
              label="Title"
              fullWidth
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              margin="normal"
              required
            />
            <TextField
              label="Content"
              fullWidth
              value={content}
              onChange={(e) => setContent(e.target.value)}
              margin="normal"
              required
            />
            <TextField
              label="Image URL"
              fullWidth
              value={imageUrl}
              onChange={(e) => setImageUrl(e.target.value)}
              margin="normal"
            />
            <CardActions sx={{ justifyContent: "center", paddingBottom: 2 }}>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                sx={{ marginTop: 2, width: "90%" }}>
                Create
              </Button>
            </CardActions>
          </form>
        </CardContent>
      </Card>
    </Container>
  );
};

export default CreatePost;
