export interface ApiResponse {
  status: number;
  message: string;
  data?: any; // Make it flexible to accommodate different data types
  errors?: { [key: string]: string[] };
}
