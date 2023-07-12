import { Box, Button } from "@mui/material";
import axios from "axios";
import { QRCodeSVG } from "qrcode.react";
import { useState } from "react";

// interface AuthResponse {
//   autoStartToken: string;
//   orderRef: string;
//   qrStartSecret: string;
//   qrStartToken: string;
// }

interface AuthResponse {
  qrCode: string;
}

const generateQrCode = (startToken: string, startSecret: string) => {
  return "";
};

function App() {
  const [authResponse, setAuthResponse] = useState<AuthResponse>();

  console.log("AuthResponse ", authResponse);

  const auth = async () => {
    const res = await axios.get("https://localhost:7139/auth");
    console.log("RES -> ", res);
  };

  const qrCode = authResponse ? authResponse : "";
  // const qrCode = authResponse
  //   ? `${authResponse?.qrStartToken}.${authResponse?.qrStartSecret}`
  //   : "";

  // const qrCode = authResponse
  //   ? `bankid.${authResponse?.qrStartToken}.${authResponse?.qrStartSecret}`
  //   : "";

  // const qrCode = authResponse
  // ? `${authResponse?.qrStartToken}.${authResponse?.qrStartSecret}`
  // : "";

  console.log("qrCode -->", qrCode);

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
