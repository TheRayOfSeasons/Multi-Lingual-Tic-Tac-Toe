#include <iostream>
#include <string>

struct player
{
    std::string token;
    std::string name;
    player(std::string _token, std::string _name) : token(_token), name(_name) {}
};

player player_1("O", "Player 1");
player player_2("X", "Player 2");

struct slot
{
    std::string empty_token = " ";
    std::string token = " ";

    bool update_token(std::string _token)
    {
        if(is_empty())
        {
            token = _token;
            return true;
        }
        return false;
    }

    bool is_empty()
    {
        return token == empty_token;
    }

    reset()
    {
        token = empty_token;
    }
};

struct board
{
    slot field[3][3];
    board()
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                slot s;
                field[i][j] = s;
            }
        }
    }

    int get_board_dimension_height()
    {
        return sizeof(field) / sizeof(field[0]);
    }

    int get_board_dimension_width()
    {
        return sizeof(field[0]) / sizeof(field[0][0]);
    }

    std::string draw(bool use_tokens)
    {
        std::string token;
        std::string printed = "";
        int height = get_board_dimension_height();
        int width = get_board_dimension_width();
        int ctr = 1;
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                if(use_tokens)
                {
                    token = field[i][j].token;
                }
                else
                {
                    token = std::to_string(ctr);
                    ctr++;
                }

                if (j == 0)
                {
                    printed += "  " + token + " | ";
                }
                else
                {
                    printed += token;
                }

                if (0 < j && j < width - 1)
                {
                    printed += " | ";
                }
            }
            printed += "\n";
            if(i < height - 1)
            {
                printed += "-------------";
                printed += "\n";
            }
        }
        return printed;
    }

    bool write_turn(player _player, int input)
    {
        int ctr = 1;
        for(int i = 0; i < get_board_dimension_width(); i++)
        {
            for(int j = 0; i < get_board_dimension_height(); j++)
            {
                if(input == ctr)
                {
                    return field[i][j].update_token(_player.token);
                }
                else
                {
                    ctr++;
                }
            }
        }
        return false;
    }

    bool equal_all_slots(slot arr[3])
    {
        bool equal = true;
        for(int i = 1; i < 3; i++)
        {
            slot current = arr[i];
            slot previous = arr[i - 1];
            if(current.token == previous.token && !current.is_empty() && !previous.is_empty())
            {
                continue;
            }
            else
            {
                equal = false;
            }
        }
        return equal;
    }

    bool check_win()
    {
        bool win = false;

        int width = get_board_dimension_width();
        int height = get_board_dimension_height();
        for(int i = 0; i < get_board_dimension_width(); i++)
        {
            slot horizontal_slots[width];
            slot vertical_slots[width];
            // Check horizontal and vertical.
            for(int j = 0; j < get_board_dimension_height(); j++)
            {
                slot h_slot;
                slot v_slot;
                h_slot = field[i][j];
                v_slot = field[j][i];
                horizontal_slots[j] = h_slot;
                vertical_slots[j] = v_slot;
            }

            bool h_win = equal_all_slots(horizontal_slots);
            bool v_win = equal_all_slots(vertical_slots);
            if(h_win || v_win)
            {
                win = true;
                break;
            }
        }

        // Check diagonal.
        slot d1[] = {field[0][0], field[1][1], field[2][2]};
        slot d2[] = {field[0][2], field[1][1], field[2][0]};
        bool d1_win = equal_all_slots(d1);
        bool d2_win = equal_all_slots(d2);
        if(d1_win || d2_win)
        {
            win = true;
        }

        return win;
    }

    bool is_full()
    {
        int width = get_board_dimension_width();
        int height = get_board_dimension_height();
        int filled;
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(!field[i][j].is_empty())
                {
                    filled++;
                }
            }
        }
        return filled == width * height;
    }

    reset()
    {
        int width = get_board_dimension_width();
        int height = get_board_dimension_height();
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                field[i][j].reset();
            }
        }
    }
};

player toggle_player(player p)
{
    if(p.token == player_1.token)
    {
        return player_2;
    }
    else
    {
        return player_1;
    }
}

clear_screen()
{
    // Make sure system is safe to use before clear screen.
    if(system( NULL )) system("CLS");
}

main()
{
    board game_board;
    player current_player = player_1;
    bool win = false;
    bool draw = false;
    bool again = true;
    std::string alert_message = "";

    while(!win && !draw && again)
    {
        clear_screen();

        std::cout << "Guide\n";
        std::cout << game_board.draw(false);
        std::cout << "\n\n";

        std::cout << "Playing Field\n";
        std::cout << game_board.draw(true) << std::endl;
        std::cout << alert_message;
        std::cout << "Turn of " << current_player.name << " [" << current_player.token << "]\n";

        int input;
        std::cout << "Input: ";
        std::cin >> input;

        bool valid_move = game_board.write_turn(current_player, input);
        win = game_board.check_win();
        if(valid_move)
        {
            if(!win)
            {
                current_player = toggle_player(current_player);
            }
            alert_message = "";
        }
        else
        {
            alert_message = "Slot " + std::to_string(input) + " is either taken or invalid.\n\n";
        }

        if(!win && game_board.is_full())
        {
            draw = true;
        }

        if(win || draw)
        {
            clear_screen();
            std::cout << game_board.draw(true);
            if(win)
            {
                std::cout << current_player.name << " has won!\n\n";
            }
            if(draw)
            {
                std::cout << "Draw!\n\n";
            }

            std::string play_again;
            std::cout << "Play again? (Y/N) : ";
            std::cin >> play_again;

            if(play_again == "Y" || play_again == "y")
            {
                win = false;
                again = true;
                draw = false;
                game_board.reset();
            }
            else
            {
                win = false;
                again = false;
                draw = false;
            }
        }
    }
}
