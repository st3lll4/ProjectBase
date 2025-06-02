import type { IDomainId } from "./IDomainId";

export interface Artist extends IDomainId {
    stageName: string;
    birthDate: string;
    isSolo: boolean;
  }