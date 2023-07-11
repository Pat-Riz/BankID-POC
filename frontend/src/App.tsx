import { Box, Button } from "@mui/material";
import axios from "axios";
import { QRCodeSVG } from "qrcode.react";
import { useState } from "react";

interface AuthResponse {
  autoStartToken: string;
  orderRef: string;
  qrStartSecret: string;
  qrStartToken: string;
}

function App() {
  const [authResponse, setAuthResponse] = useState<AuthResponse>();

  const auth = async () => {
    const res = await axios.get("https://localhost:7139/auth");
    console.log("RES -> ", res);
  };

  const qrCode = authResponse
    ? `${authResponse?.qrStartToken} ${authResponse?.qrStartSecret}`
    : "HEJ";

  return (
    <>
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <Button
          onClick={() => {
            void (async () => {
              const res = await axios.get("https://localhost:7139/auth");
              console.log("RES -> ", res);
              setAuthResponse(res.data as AuthResponse);
            })();
          }}
        >
          Auth
        </Button>
        <QRCodeSVG value={qrCode}></QRCodeSVG>
      </Box>
    </>
  );
}

export default App;
