import type { LoginDTO } from "@/types/Identity/ILoginDTO";
import type { ResultObject } from "@/types/IResultObject";
import { BaseService } from "./BaseService";
import type { RegisterDTO } from "@/types/Identity/IRegisterDTO";
import type { AxiosError } from "axios";

export class IdentityService extends BaseService {
  
	async registerAsync(
    email: string,
    password: string,
    firstname: string,
    lastname: string,
  ): Promise<ResultObject<LoginDTO>> {
    const url = "account/register";
    try {
      const registerData = {
        email,
        password, 
        firstname,
        lastname
      };

      const response = await this.axiosInstance.post<RegisterDTO>(
        url,
        registerData
      );

      console.log('register response', response)

      if (response.status <= 300) {
        return {
          statusCode: response.status,
          data: response.data,
        };
      }

      return {
        statusCode: response.status,
        errors: [
          (response.status.toString() + " " + response.statusText).trim(),
        ],
      };
    } catch (error) {
      console.log("error!!!: ", (error as Error).message);

      return {
        statusCode: (error as AxiosError)?.status,
        errors: [(error as AxiosError).code ?? ""],
      };
    }
  }

  async loginAsync(
    email: string,
    password: string
  ): Promise<ResultObject<LoginDTO>> {
    const url = "account/login";
    try {
      const loginData = {
        email,
        password
      };

      const response = await this.axiosInstance.post<LoginDTO>(
        url + "?jwtExpiresInSeconds=10",
        loginData
      );

      console.log('login response', response)

      if (response.status <= 300) {
        return {
          statusCode: response.status,
          data: response.data,
        };
      }

      return {
        statusCode: response.status,
        errors: [
          (response.status.toString() + " " + response.statusText).trim(),
        ],
      };
    } catch (error) {
      console.log("error!!!: ", (error as Error).message);

      return {
        statusCode: (error as AxiosError)?.status,
        errors: [(error as AxiosError).code ?? ""],
      };
    }
  }

  async logoutAsync(
    refreshToken: string,
    jwt: string
  ): Promise<ResultObject<Response>> {
    const url = "account/logout";
    try {
      const logoutData = {
        refreshToken,
      };

      const response = await this.axiosInstance.post<Response>(url, logoutData);

      //console.log('logout response', response)

      if (response.status <= 300) {
        return {
          statusCode: response.status,
          data: response.data,
        };
      }

      return {
        statusCode: response.status,
        errors: [
          (response.status.toString() + " " + response.statusText).trim(),
        ],
      };
    } catch (error) {
      return {
        statusCode: (error as AxiosError)?.status,
        errors: [(error as AxiosError).code ?? ""],
      };
    }
  }
}
