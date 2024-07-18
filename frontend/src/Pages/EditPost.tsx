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
  const [imageUrl, setImageUrl] = useState("");

  useEffect(() => {
    const fetchPost = async () => {
      if (id) {
        const response = await getPostById(Number(id));
        setTitle(response.data.title);
        setContent(response.data.content);
        setImageUrl(response.data.imageUrl || "");
      }
    };

    fetchPost();
  }, [id]);

  const handleUpdate = async (event: React.FormEvent) => {
    event.preventDefault();
    if (id) {
      await updatePost(Number(id), {
        id: Number(id),
        title,
        content,
        imageUrl,
      });
      navigate(`/posts/${id}`);
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
            <TextField
              label="Image URL"
              fullWidth
              value={imageUrl}
              onChange={(e) => setImageUrl(e.target.value)}
              margin="normal"
              required
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
