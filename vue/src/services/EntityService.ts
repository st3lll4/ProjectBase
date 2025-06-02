import type { IDomainId } from "@/types/IDomainId";
import { AxiosError } from "axios";
import { BaseService } from "./BaseService";
import type { ResultObject } from "@/types/IResultObject";


export abstract class EntityService<
  TEntity extends IDomainId,
  TRequestEntity
> extends BaseService {
  constructor(private basePath: string) {
    super();
  }

  async getAllAsync(): Promise<ResultObject<TEntity[]>> {
    try {
      const response = await this.axiosInstance.get<TEntity[]>(this.basePath);

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
      console.log("error: ", (error as AxiosError).message);
      return {
        statusCode: (error as AxiosError).status ?? 0,
        errors: [(error as AxiosError).code ?? "???"],
      };
    }
  }

  async getAsync(id: string): Promise<ResultObject<TEntity>> {
    try {
      const response = await this.axiosInstance.get<TEntity>(
        this.basePath + "/" + id
      );

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
      console.log("error: ", (error as AxiosError).message);
      return {
        statusCode: (error as AxiosError).status ?? 0,
        errors: [(error as AxiosError).code ?? "???"],
      };
    }
  }

  async addAsync(entity: TRequestEntity): Promise<ResultObject<TEntity>> {
    try {
      const response = await this.axiosInstance.post<TEntity>(
        this.basePath,
        entity
      );

      console.log(response)

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
      console.log("error: ", (error as AxiosError).message);
      return {
        statusCode: (error as AxiosError).status ?? 0,
        errors: [(error as AxiosError).code ?? "???"],
      };
    }
  }

  async deleteAsync(id: string): Promise<ResultObject<null>> {
    try {
      const response = await this.axiosInstance.delete<null>(
        this.basePath + "/" + id
      );

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
      console.log("error: ", (error as AxiosError).message);
      return {
        statusCode: (error as AxiosError).status ?? 0,
        errors: [(error as AxiosError).code ?? "???"],
      };
    }
  }

  async updateAsync(entity: TEntity): Promise<ResultObject<TEntity>> {
    try {
      const response = await this.axiosInstance.put<TEntity>(
        this.basePath + "/" + entity.id,
        entity
      );

      console.log("login response", response);

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
      console.log("error: ", (error as AxiosError).message);
      return {
        statusCode: (error as AxiosError).status ?? 0,
        errors: [(error as AxiosError).code ?? "???"],
      };
    }
  }
}
