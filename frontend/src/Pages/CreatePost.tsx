import React, { useState } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Card,
  CardContent,
  CardActions,
  Grid,
  Box,
  IconButton,
  CircularProgress,
} from "@mui/material";
import DriveFolderUploadIcon from "@mui/icons-material/DriveFolderUpload";
import DeleteIcon from "@mui/icons-material/Delete";
import { addPost } from "../Services/Api";
import { useNavigate } from "react-router-dom";

const CreatePost: React.FC = () => {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [images, setImages] = useState<File[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errors, setErrors] = useState({ title: false, content: false });
  const navigate = useNavigate();

    const validate = () => {
    let valid = true;
    const newErrors = { title: false, content: false };
    if (!title.trim()) {
      newErrors.title = true;
      valid = false;
    }
    if (!content.trim()) {
      newErrors.content = true;
      valid = false;
    }
    setErrors(newErrors);
    return valid;
    };
  
    const handleInputChange = (setter: React.Dispatch<React.SetStateAction<string>>, value: string) => {
      setter(value);
      validate();
    };
  
    const handleSubmit = async (event: React.FormEvent) => {
      event.preventDefault();
      if (!validate()) return;
      setIsSubmitting(true); // Start loading

    const formData = new FormData();
    formData.append("title", title);
    formData.append("content", content);

    images.forEach((image) => {
      formData.append("images", image);
    });

    try {
      const newPost = await addPost(formData);
      navigate(`/posts/${newPost.data.id}`);
    } catch (error) {
      console.error("Failed to create post:", error);
      alert("Failed to create post. Please try again.");
    } finally {
      setIsSubmitting(false); // Stop loading
    }
  };
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setImages(e.target.files ? Array.from(e.target.files) : []);
  };

  const handleButtonClick = () => {
    const fileInput = document.getElementById("file-input") as HTMLInputElement;
    if (fileInput) {
      fileInput.click();
    }
  };

  const handleImageDelete = (index: number) => {
    const newImages = images.filter((_, i) => i !== index);
    setImages(newImages);
  };

  return (
    <Container maxWidth="sm" sx={{ marginTop: 4 }}>
      <Card>
        <CardContent>
          <Typography variant="h4" gutterBottom>
            Create New Post
          </Typography>
          <form onSubmit={handleSubmit} encType="multipart/form-data" noValidate>
            <TextField
              label="Title"
              fullWidth
              value={title}
              onChange={(e) => handleInputChange(setTitle, e.target.value)}
              margin="normal"
              required
              error={errors.title}
              helperText={errors.title ? "Title is required" : ""}
            />
            <TextField
              label="Content"
              fullWidth
              value={content}
              onChange={(e) => handleInputChange(setContent, e.target.value)}
              margin="normal"
              required
              multiline
              rows={6}
              error={errors.content}
              helperText={errors.content ? "Content is required" : ""}
            />
            <Grid container justifyContent="center" sx={{ marginTop: 2 }}>
              <Button
                component="label"
                variant="contained"
                startIcon={<DriveFolderUploadIcon />}
                onClick={handleButtonClick}
                sx={{
                  justifyContent: "center",
                  fontSize: "small",
                }}>
                Upload Image
              </Button>
              <input
                id="file-input"
                type="file"
                accept="image/*"
                multiple
                onChange={handleFileChange}
                style={{ display: "none" }}
              />
            </Grid>
            {images.length > 0 && (
              <Box mt={2}>
                <Grid container>
                  {images.map((image, index) => (
                    <Grid item xs={3} key={index}>
                      <div
                        style={{
                          position: "relative",
                          display: "inline-block",
                          margin: "5px",
                        }}>
                        <img
                          src={URL.createObjectURL(image)}
                          alt="Thumbnail"
                          style={{ width: "100px", height: "100px" }}
                        />
                        <IconButton
                          onClick={() => handleImageDelete(index)}
                          style={{
                            position: "absolute",
                            top: "0",
                            right: "0",
                            backgroundColor: "rgba(255, 255, 255, 0.7)",
                          }}>
                          <DeleteIcon />
                        </IconButton>
                      </div>
                    </Grid>
                  ))}
                </Grid>
              </Box>
            )}
            <CardActions sx={{ justifyContent: "center" }}>
              <Button
                type="submit"
                variant="contained"
                sx={{ marginTop: 2, width: "100%" }}
                disabled={isSubmitting} // Disable button while submitting
              >
                {isSubmitting ? <CircularProgress size={24} /> : "Create"}
              </Button>
            </CardActions>
          </form>
        </CardContent>
      </Card>
    </Container>
  );
};

export default CreatePost;