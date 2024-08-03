import React, { useEffect, useState } from "react";
import { Box, Container, Grid, Paper, Typography } from "@mui/material";
import { getPosts } from "../Services/Api";
import { Post } from "../Models/Post";
import { Link } from "react-router-dom";

const Home: React.FC = () => {
  const [posts, setPosts] = useState<Post[]>([]);

  useEffect(() => {
    const fetchPosts = async () => {
      const response = await getPosts();
      setPosts(response.data);
    };

    fetchPosts();
  }, []);

  return (
    <Container>
      <Typography
        sx={{
          paddingY: 4,
          fontFamily: "Josefin Sans, sans-serif",
          fontStyle: "normal",
        }}
        variant="h4"
        align="center"
        gutterBottom>
        Welcome to the cutest pet blog (っ^_^)っ
      </Typography>
      <Grid container spacing={6}>
        {posts.map((post) => (
          <Grid item xs={12} sm={6} md={4} key={post.id}>
            <Link
              to={`/posts/${post.id}`}
              style={{ textDecoration: "none", color: "inherit" }}>
              <Box>
                <Paper elevation={8} style={{ padding: "16px" }}>
                  {post.imageUrls.length > 0 && (
                    <img
                      src={post.imageUrls[0]}
                      alt={post.title}
                      style={{
                        width: "100%",
                        height: "350px",
                        objectFit: "cover",
                      }}
                    />
                  )}
                  <Typography variant="h6">{post.title}</Typography>
                </Paper>
              </Box>
            </Link>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
};

export default Home;