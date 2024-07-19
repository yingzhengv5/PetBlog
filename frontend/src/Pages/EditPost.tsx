import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  Container,
  TextField,
  Button,
  Typography,
  Card,
  CardContent,
  CardActions,
} from "@mui/material";
import { getPostById, updatePost } from "../Services/Api";

const EditPost: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [images, setImages] = useState<File[]>([]);
  const [existingImages, setExistingImages] = useState<string[]>([]);

  useEffect(() => {
    const fetchPost = async () => {
      if (id) {
        const response = await getPostById(Number(id));
        setTitle(response.data.title);
        setContent(response.data.content);
        setExistingImages(response.data.imageUrls || []);
      }
    };

    fetchPost();
  }, [id]);

  const handleUpdate = async (event: React.FormEvent) => {
    event.preventDefault();
    if (id) {
      const formData = new FormData();
      formData.append("id", id);
      formData.append("title", title);
      formData.append("content", content);

      // Add existing images
      existingImages.forEach((url) => {
        formData.append("existingImages", url);
      });

      // Add new images
      images.forEach((image) => {
        formData.append("images", image);
      });

      try {
        await updatePost(Number(id), formData);
        navigate(`/posts/${id}`);
      } catch (error) {
        console.error("Failed to update post:", error);
        alert("Failed to update post. Please try again.");
      }
    }
  };

  return (
    <Container maxWidth="sm" sx={{ marginTop: 4 }}>
      <Card>
        <CardContent>
          <Typography variant="h4" gutterBottom>
            Edit Post
          </Typography>
          <form onSubmit={handleUpdate}>
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
            <input
              type="file"
              accept="image/*"
              multiple
              onChange={(e) =>
                setImages(e.target.files ? Array.from(e.target.files) : [])
              }
            />
            <CardActions sx={{ justifyContent: "center", paddingBottom: 2 }}>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                sx={{ marginTop: 2, width: "90%" }}>
                Update
              </Button>
            </CardActions>
          </form>
        </CardContent>
      </Card>
    </Container>
  );
};

export default EditPost;
