import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import { ThemeProviderWrapper } from "./Contexts/ThemeContext.tsx";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <ThemeProviderWrapper>
      <App />
    </ThemeProviderWrapper>
  </React.StrictMode>
);
