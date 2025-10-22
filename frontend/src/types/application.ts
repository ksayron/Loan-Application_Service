export enum EApplicationStatus {
  Published = 0,
  Unpublished = 1
}

export interface ApplicationDto {
  number: string
  amount: number
  termValue: number
  interestValue: number
  status: EApplicationStatus | string
  createdAt: string
  modifiedAt: string
}

export interface CreateApplicationDto {
  number: string
  amount: number
  termValue: number
  interestValue: number
}
