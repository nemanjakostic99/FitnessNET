export interface FriendRequestDTO {
    name: string;
    surname: string;
    username: string;
    isTrainer: boolean;
    profilePicture?: {
      contentType: string;
      pictureData: string;
    } | null;
  } 