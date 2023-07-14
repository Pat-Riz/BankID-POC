import { Box, Button, Typography } from "@mui/material";
import axios from "axios";
import { QRCodeSVG } from "qrcode.react";
import { useEffect, useState } from "react";

interface AuthResponse {
  orderRef: string;
  qrCode: string;
}

interface CollectResponse {
  qrCode: string;
  status: string;
  hintCode: string;
}

const TEST_IP = "172.17.208.75";

function App() {
  const [authResponse, setAuthResponse] = useState<AuthResponse>();
  const [collectResponse, setCollectResponse] = useState<CollectResponse>();
  const [timer, setTimer] = useState<number | undefined>(undefined);

  const stopTimer = () => {
    if (timer) {
      clearInterval(timer);
      setTimer(undefined);
    }
  };

  useEffect(() => {
    const collect = async () => {
      const res = await axios.post("https://localhost:7139/collect", {
        orderRef: authResponse?.orderRef,
      });
      const data = res.data as CollectResponse;
      if (data.status !== "pending") {
        console.log("clearing Timer");

        stopTimer(); //This dont work for some reason :(
      }
      setCollectResponse(data);
    };

    if (authResponse?.orderRef) {
      setTimer(setInterval(collect, 1000));
    }

    return () => {
      stopTimer();
    };
  }, [authResponse]);

  const qrCode = collectResponse
    ? collectResponse.qrCode
    : authResponse?.qrCode;

  // console.log("Collect Status -->", collectResponse?.status);
  console.log("qrCode -->", qrCode);

  return (
    <>
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          flexDirection: "column",
        }}
      >
        <Button
          onClick={() => {
            void (async () => {
              const res = await axios.post("https://localhost:7139/auth", {
                endUserIp: TEST_IP,
              });
              setAuthResponse(res.data as AuthResponse);
            })();
          }}
          variant='contained'
          sx={{ my: 8 }}
        >
          Logga in
        </Button>

        {qrCode && (
          <>
            <Typography>Scan it bitch</Typography>
            <Box sx={{ borderColor: "black", border: 2, p: 8 }}>
              <QRCodeSVG value={qrCode}></QRCodeSVG>{" "}
            </Box>
          </>
        )}
        <Typography>Status: {collectResponse?.status}</Typography>
        <Typography>HintCode: {collectResponse?.hintCode}</Typography>
      </Box>
    </>
  );
}

export default App;
