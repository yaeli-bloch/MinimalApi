const express = require('express');
const axios = require('axios');
const app = express();
const port = process.env.PORT;


app.get('/apps', async (req, res) => {
    try {
        const response = await axios.get('https://api.render.com/v1/services', {
            headers: {
                'Authorization': `Bearer rnd_wHQAOSawLsZD5AeQgbB8jB5MhJgv`
            }
        });
        res.json(response.data);  // מחזיר את המידע שנמצא ב-Render
    } catch (error) {
        res.status(500).json({ error: 'error : in loading the data '});
    }
});

app.listen(port, () => {
    console.log(`Server listening at http://localhost:${port}`);
});
