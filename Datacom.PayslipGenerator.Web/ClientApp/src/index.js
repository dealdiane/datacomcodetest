import { createRoot } from 'react-dom/client';

import App from "./components/App";
import './styles/main.scss';

const container = document.getElementById("app");

createRoot(container).render(<App />);