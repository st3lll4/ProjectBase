import type { LoginDTO } from "@/types/Identity/ILoginDTO";
import axios, { type AxiosInstance } from "axios";

export abstract class BaseService {
  protected axiosInstance: AxiosInstance;

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: "http://localhost:5121/api/v1.0/",
      headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
      },
    });

    this.axiosInstance.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem("_jwt");
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        console.log(error + "this mf error!");
        return Promise.reject(error);
      }
    );

    this.axiosInstance.interceptors.response.use(
      (response) => {
        return response;
      },

      async (error) => {
        const originalRequest = error.config;
        if (
          error.response &&
          error.response.status === 401 &&
          !originalRequest._retry
        ) {
          originalRequest._retry = true;
          try {
            const jwt = localStorage.getItem("_jwt");
            const refreshToken = localStorage.getItem("_refreshToken");

            const response = await axios.post<LoginDTO>(
              "http://localhost:5014/api/v1.0/account/RenewRefreshToken?jwtexpiresinseconds=10",
              {
                jwt: jwt,
                refreshToken: refreshToken,
              }
            );

            console.log("renewRefreshToken", response);

            if (response && response.status <= 300) {
              localStorage.setItem("_jwt", response.data.jwt);
              localStorage.setItem("_refreshToken", response.data.refreshToken);
              originalRequest.headers.Authorization = `Bearer ${response.data.jwt}`;

              return this.axiosInstance(originalRequest);
            }
            localStorage.setItem("_jwt", "");
            localStorage.setItem("_refreshToken", "");

            return Promise.reject(error);
          } catch (error) {
            console.error("Error refreshing token:", error);

            // delete token from localstorage bc refreshtoken isnt valid
            localStorage.setItem("_jwt", "");
            localStorage.setItem("_refreshToken", "");
            return Promise.reject(error);
          }
        }
        return Promise.reject(error);
      }
    );
  }
}
