import { AppBar, Toolbar, Typography, Button, IconButton } from "@mui/material";
import { Link } from "react-router-dom";
import { useThemeContext } from "../Contexts/ThemeContext";
import Brightness4Icon from "@mui/icons-material/Brightness4";
import Brightness7Icon from "@mui/icons-material/Brightness7";

const Navbar: React.FC = () => {
  const { darkMode, toggleDarkMode } = useThemeContext();
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h3" sx={{ flexGrow: 1 }}>
          <Button color="inherit" component={Link} to="/">
            PetBlog
          </Button>
          <Button color="inherit" component={Link} to="/create">
            Create Post
          </Button>
        </Typography>
        <IconButton color="inherit" onClick={toggleDarkMode}>
          {darkMode ? <Brightness7Icon /> : <Brightness4Icon />}
        </IconButton>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;
