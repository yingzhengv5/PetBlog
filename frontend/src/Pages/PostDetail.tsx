import { useNavigate, useParams } from "react-router-dom";
import { deletePost, getPostById } from "../Services/Api";
import { Post } from "../Models/Post";
import { useEffect, useState } from "react";
import { Box, Button, Grid, Paper, Typography } from "@mui/material";
import { format } from "date-fns";

const PostDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [post, setPost] = useState<Post | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPost = async () => {
      if (id) {
        const fetchPost = await getPostById(Number(id));
        setPost(fetchPost.data);
      }
    };

    fetchPost();
  }, [id]);

  const handleEdit = () => {
    navigate(`/edit/${id}`);
  };

  const handleDelete = async () => {
    if (id && window.confirm("Are you sure you want to delete this post?")) {
      await deletePost(Number(id));
      navigate("/");
    }
  };

  if (!post) {
    return <Typography>Loading...</Typography>;
  }

  return (
    <Paper elevation={4} style={{ padding: "16px" }}>
      <Grid container spacing={4}>
        <Grid item xs={12} md={4}>
          <img
            src={post.imageUrl}
            alt={post.title}
            style={{ width: "100%", height: "auto", borderRadius: "8px" }}
          />
        </Grid>
        <Grid item xs={12} md={8}>
          <Box>
            <Typography variant="h4" gutterBottom>
              {post.title}
            </Typography>
            <Typography variant="subtitle1" gutterBottom>
              {`${format(new Date(post.createAt), "yyyy-MM-dd")}`}
            </Typography>
          </Box>
          <Box
            border={2}
            borderColor="grey.400"
            borderRadius={2}
            padding={2}
            marginBottom={2}>
            <Typography variant="body1" paragraph>
              {post.content}
            </Typography>
          </Box>
          <Box>
            <Button
              variant="contained"
              color="primary"
              onClick={handleEdit}
              style={{ marginRight: "8px" }}>
              Edit
            </Button>
            <Button
              variant="contained"
              color="secondary"
              onClick={handleDelete}>
              Delete
            </Button>
          </Box>
        </Grid>
      </Grid>
    </Paper>
  );
};

export default PostDetail;
