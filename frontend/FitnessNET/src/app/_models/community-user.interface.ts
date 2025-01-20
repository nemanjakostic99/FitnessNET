export interface CommunityUser {
  id: number;
  name: string;
  surname: string;
  username: string;
  gender: string;
  isTrainer: boolean;
  rating: number | null;
  description: string;
  profilePicture?: {
    contentType: string;
    pictureData: string;
  } | null;
} 