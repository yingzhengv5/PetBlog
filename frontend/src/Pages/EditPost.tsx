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
  IconButton,
  Grid,
  Box,
  CircularProgress,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import DriveFolderUploadIcon from "@mui/icons-material/DriveFolderUpload";
import { getPostById, updatePost } from "../Services/Api";

const EditPost: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [images, setImages] = useState<File[]>([]);
  const [existingImages, setExistingImages] = useState<string[]>([]);
  const [deletedImages, setDeletedImages] = useState<string[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false); // Add state for loading
  const [errors, setErrors] = useState({ title: false, content: false });

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
    if (!validate()) return;
    
    if (id) {
      setIsSubmitting(true); // Start loading

      const formData = new FormData();
      formData.append("id", id);
      formData.append("title", title);
      formData.append("content", content);

      // Add existing images
      existingImages.forEach((url) => {
        formData.append("existingImages", url);
      });

      deletedImages.forEach((url) => {
        formData.append("deletedImages", url);
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

  const handleExistingImageDelete = (url: string) => {
    setExistingImages(existingImages.filter((image) => image !== url));
    setDeletedImages([...deletedImages, url]);
  };

  return (
    <Container maxWidth="sm" sx={{ marginTop: 4 }}>
      <Card>
        <CardContent>
          <Typography variant="h4" gutterBottom>
            Edit Post
          </Typography>
          <form onSubmit={handleUpdate} encType="multipart/form-data" noValidate>
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
              error={errors.content}
              helperText={errors.content ? "Content is required" : ""}
            />
            <Button
              component="label"
              variant="contained"
              startIcon={<DriveFolderUploadIcon />}
              onClick={handleButtonClick}
              sx={{
                justifyContent: "center",
                fontSize: "small",
                marginY: 2,
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
            <Box mt={2}>
              <Grid container>
                {images.length > 0 &&
                  images.map((image, index) => (
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
                {existingImages.length > 0 &&
                  existingImages.map((url) => (
                    <Grid item xs={3} key={url}>
                      <div
                        style={{
                          position: "relative",
                          display: "inline-block",
                          margin: "5px",
                        }}>
                        <img
                          src={url}
                          alt="Thumbnail"
                          style={{ width: "100px", height: "100px" }}
                        />
                        <IconButton
                          onClick={() => handleExistingImageDelete(url)}
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
            <CardActions sx={{ justifyContent: "center", paddingBottom: 2 }}>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                sx={{ marginTop: 2, width: "90%" }}
                disabled={isSubmitting}>
                {isSubmitting ? <CircularProgress size={24} /> : "Update"}
              </Button>
            </CardActions>
          </form>
        </CardContent>
      </Card>
    </Container>
  );
};

export default EditPost;