export interface ServiceCreateRequest {
  readonly name: string;
  readonly description: string;
  readonly price: number;
  readonly duration: Date;
}

export interface ServiceUpdateRequest {
  readonly id: string;
  readonly name: string;
  readonly description: string;
  readonly price: number;
  readonly duration: Date;
  readonly isActive: boolean;
}
