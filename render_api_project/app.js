import express from 'express';
import axios from 'axios';

const app = express();
const PORT = process.env.PORT || 3000;
// ה-API Key שלך (שימי כאן את המפתח שלך)
const API_KEY = 'rnd_wHQAOSawLsZD5AeQgbB8jB5MhJgv';

// הגדרת ה-Endpoint
app.get('/', async (req, res) => {
  try {
    // קריאת ה-Render API כדי לקבל את רשימת השירותים
    const response = await axios.get('https://api.render.com/v1/services', {
      headers: {
        Authorization: `Bearer ${API_KEY}`
      }
    });

    // החזרת הנתונים כ-JSON
    res.json(response.data);
  } catch (error) {
    console.error(error);
    res.status(500).json({ error: 'Failed to fetch services from Render API' });
  }
});

app.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});
