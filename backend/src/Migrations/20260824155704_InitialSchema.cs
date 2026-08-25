using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeksanaStudio.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "add_on",
                columns: table => new
                {
                    add_on_id = table.Column<Guid>(type: "uuid", nullable: false),
                    add_on_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    add_on_order = table.Column<int>(type: "integer", nullable: false),
                    add_on_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    add_on_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    add_on_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    add_on_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    add_on_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    add_on_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    add_on_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_add_on", x => x.add_on_id);
                });

            migrationBuilder.CreateTable(
                name: "locale",
                columns: table => new
                {
                    locale_id = table.Column<Guid>(type: "uuid", nullable: false),
                    locale_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    locale_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    locale_nativename = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    locale_isdefault = table.Column<bool>(type: "boolean", nullable: false),
                    locale_isactive = table.Column<bool>(type: "boolean", nullable: false),
                    locale_order = table.Column<int>(type: "integer", nullable: false),
                    locale_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    locale_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    locale_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    locale_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    locale_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    locale_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    locale_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locale", x => x.locale_id);
                });

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    media_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_objectpath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    media_mime = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    media_sizebytes = table.Column<long>(type: "bigint", nullable: false),
                    media_width = table.Column<int>(type: "integer", nullable: true),
                    media_height = table.Column<int>(type: "integer", nullable: true),
                    media_originalname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    media_label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    media_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    media_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    media_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    media_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    media_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    media_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    media_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media", x => x.media_id);
                });

            migrationBuilder.CreateTable(
                name: "menu",
                columns: table => new
                {
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    menu_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    menu_customevents = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    menu_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    menu_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    menu_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    menu_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    menu_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    menu_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    menu_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu", x => x.menu_id);
                });

            migrationBuilder.CreateTable(
                name: "note",
                columns: table => new
                {
                    note_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    note_pillar = table.Column<string>(type: "text", nullable: false),
                    note_order = table.Column<int>(type: "integer", nullable: false),
                    note_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    note_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    note_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    note_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    note_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    note_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    note_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note", x => x.note_id);
                });

            migrationBuilder.CreateTable(
                name: "page_copy",
                columns: table => new
                {
                    page_copy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    page_copy_pagecode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    page_copy_order = table.Column<int>(type: "integer", nullable: false),
                    page_copy_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    page_copy_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    page_copy_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_copy_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_copy_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    page_copy_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    page_copy_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page_copy", x => x.page_copy_id);
                });

            migrationBuilder.CreateTable(
                name: "page_copy_slot_definition",
                columns: table => new
                {
                    page_copy_slot_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    page_copy_slot_definition_pagecode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    page_copy_slot_definition_slotkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    page_copy_slot_definition_label = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    page_copy_slot_definition_hint = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    page_copy_slot_definition_kind = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    page_copy_slot_definition_maxlength = table.Column<int>(type: "integer", nullable: false),
                    page_copy_slot_definition_required = table.Column<bool>(type: "boolean", nullable: false),
                    page_copy_slot_definition_group = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    page_copy_slot_definition_order = table.Column<int>(type: "integer", nullable: false),
                    page_copy_slot_definition_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    page_copy_slot_definition_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    page_copy_slot_definition_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_copy_slot_definition_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_copy_slot_definition_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    page_copy_slot_definition_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    page_copy_slot_definition_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page_copy_slot_definition", x => x.page_copy_slot_definition_id);
                });

            migrationBuilder.CreateTable(
                name: "page_document",
                columns: table => new
                {
                    page_document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    page_document_pagecode = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    page_document_order = table.Column<int>(type: "integer", nullable: false),
                    page_document_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    page_document_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    page_document_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_document_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_document_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    page_document_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    page_document_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page_document", x => x.page_document_id);
                });

            migrationBuilder.CreateTable(
                name: "payment_term",
                columns: table => new
                {
                    payment_term_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_term_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    payment_term_order = table.Column<int>(type: "integer", nullable: false),
                    payment_term_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    payment_term_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    payment_term_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    payment_term_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    payment_term_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    payment_term_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    payment_term_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_term", x => x.payment_term_id);
                });

            migrationBuilder.CreateTable(
                name: "process_step",
                columns: table => new
                {
                    process_step_id = table.Column<Guid>(type: "uuid", nullable: false),
                    process_step_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    process_step_step = table.Column<int>(type: "integer", nullable: false),
                    process_step_order = table.Column<int>(type: "integer", nullable: false),
                    process_step_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    process_step_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    process_step_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    process_step_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    process_step_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    process_step_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    process_step_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process_step", x => x.process_step_id);
                });

            migrationBuilder.CreateTable(
                name: "project_phase",
                columns: table => new
                {
                    project_phase_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_phase_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    project_phase_step = table.Column<int>(type: "integer", nullable: false),
                    project_phase_order = table.Column<int>(type: "integer", nullable: false),
                    project_phase_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    project_phase_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    project_phase_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    project_phase_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    project_phase_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    project_phase_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    project_phase_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_phase", x => x.project_phase_id);
                });

            migrationBuilder.CreateTable(
                name: "service_package",
                columns: table => new
                {
                    service_package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_package_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    service_package_group = table.Column<string>(type: "text", nullable: false),
                    service_package_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    service_package_price = table.Column<decimal>(type: "numeric", nullable: false),
                    service_package_highlighted = table.Column<bool>(type: "boolean", nullable: false),
                    service_package_order = table.Column<int>(type: "integer", nullable: false),
                    service_package_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    service_package_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    service_package_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    service_package_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    service_package_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_package_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    service_package_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_package", x => x.service_package_id);
                });

            migrationBuilder.CreateTable(
                name: "site_profile",
                columns: table => new
                {
                    site_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_profile_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    site_profile_legalname = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    site_profile_ownername = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    site_profile_email = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    site_profile_whatsappnumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    site_profile_whatsappdisplay = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    site_profile_linkedin = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    site_profile_instagram = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    site_profile_github = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    site_profile_revisionrounds = table.Column<int>(type: "integer", nullable: false),
                    site_profile_warrantydays = table.Column<int>(type: "integer", nullable: false),
                    site_profile_updateeverydays = table.Column<int>(type: "integer", nullable: false),
                    site_profile_replywithinhours = table.Column<int>(type: "integer", nullable: false),
                    site_profile_quotevaliddays = table.Column<int>(type: "integer", nullable: false),
                    site_profile_pricefloor = table.Column<decimal>(type: "numeric", nullable: false),
                    site_profile_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    site_profile_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    site_profile_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    site_profile_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    site_profile_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    site_profile_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    site_profile_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_profile", x => x.site_profile_id);
                });

            migrationBuilder.CreateTable(
                name: "slug_history",
                columns: table => new
                {
                    slug_history_id = table.Column<Guid>(type: "uuid", nullable: false),
                    slug_history_entitytype = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    slug_history_entityid = table.Column<Guid>(type: "uuid", nullable: false),
                    slug_history_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    slug_history_oldslug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug_history_newslug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug_history_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    slug_history_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    slug_history_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    slug_history_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    slug_history_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    slug_history_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    slug_history_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_slug_history", x => x.slug_history_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    user_password = table.Column<string>(type: "text", nullable: true),
                    user_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    user_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "add_on_translation",
                columns: table => new
                {
                    add_on_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    add_on_translation_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    add_on_translation_price = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    add_on_translation_appliesto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    add_on_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    add_on_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    add_on_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    add_on_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    add_on_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    add_on_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    add_on_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    add_on_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    add_on_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    add_on_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    add_on_translation_status = table.Column<string>(type: "text", nullable: false),
                    add_on_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_add_on_translation", x => x.add_on_translation_id);
                    table.ForeignKey(
                        name: "FK_add_on_translation_add_on_add_on_translation_parentid",
                        column: x => x.add_on_translation_parentid,
                        principalTable: "add_on",
                        principalColumn: "add_on_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "case_study",
                columns: table => new
                {
                    case_study_id = table.Column<Guid>(type: "uuid", nullable: false),
                    case_study_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    case_study_label = table.Column<string>(type: "text", nullable: false),
                    case_study_figure = table.Column<string>(type: "text", nullable: false),
                    case_study_covermediaid = table.Column<Guid>(type: "uuid", nullable: true),
                    case_study_year = table.Column<int>(type: "integer", nullable: false),
                    case_study_stack = table.Column<string>(type: "jsonb", nullable: true),
                    case_study_order = table.Column<int>(type: "integer", nullable: false),
                    case_study_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    case_study_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    case_study_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    case_study_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    case_study_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    case_study_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    case_study_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_study", x => x.case_study_id);
                    table.ForeignKey(
                        name: "FK_case_study_media_case_study_covermediaid",
                        column: x => x.case_study_covermediaid,
                        principalTable: "media",
                        principalColumn: "media_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    role_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    role_description = table.Column<string>(type: "text", nullable: true),
                    role_order = table.Column<int>(type: "integer", nullable: true),
                    role_defaultmenu_id = table.Column<Guid>(type: "uuid", nullable: true),
                    role_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    role_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    role_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    role_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    role_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.role_id);
                    table.ForeignKey(
                        name: "FK_role_menu_role_defaultmenu_id",
                        column: x => x.role_defaultmenu_id,
                        principalTable: "menu",
                        principalColumn: "menu_id");
                });

            migrationBuilder.CreateTable(
                name: "note_translation",
                columns: table => new
                {
                    note_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note_translation_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    note_translation_summary = table.Column<string>(type: "text", nullable: true),
                    note_translation_body = table.Column<string>(type: "jsonb", nullable: true),
                    note_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    note_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    note_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    note_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    note_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    note_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    note_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    note_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    note_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    note_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    note_translation_status = table.Column<string>(type: "text", nullable: false),
                    note_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note_translation", x => x.note_translation_id);
                    table.ForeignKey(
                        name: "FK_note_translation_note_note_translation_parentid",
                        column: x => x.note_translation_parentid,
                        principalTable: "note",
                        principalColumn: "note_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "page_copy_translation",
                columns: table => new
                {
                    page_copy_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    page_copy_translation_slots = table.Column<string>(type: "jsonb", nullable: true),
                    page_copy_translation_metatitle = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    page_copy_translation_metadescription = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    page_copy_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    page_copy_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    page_copy_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_copy_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_copy_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    page_copy_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    page_copy_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    page_copy_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    page_copy_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    page_copy_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    page_copy_translation_status = table.Column<string>(type: "text", nullable: false),
                    page_copy_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page_copy_translation", x => x.page_copy_translation_id);
                    table.ForeignKey(
                        name: "FK_page_copy_translation_page_copy_page_copy_translation_paren~",
                        column: x => x.page_copy_translation_parentid,
                        principalTable: "page_copy",
                        principalColumn: "page_copy_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "page_document_translation",
                columns: table => new
                {
                    page_document_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    page_document_translation_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    page_document_translation_lead = table.Column<string>(type: "text", nullable: true),
                    page_document_translation_body = table.Column<string>(type: "jsonb", nullable: true),
                    page_document_translation_metatitle = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    page_document_translation_metadescription = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    page_document_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    page_document_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    page_document_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_document_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    page_document_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    page_document_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    page_document_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    page_document_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    page_document_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    page_document_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    page_document_translation_status = table.Column<string>(type: "text", nullable: false),
                    page_document_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page_document_translation", x => x.page_document_translation_id);
                    table.ForeignKey(
                        name: "FK_page_document_translation_page_document_page_document_trans~",
                        column: x => x.page_document_translation_parentid,
                        principalTable: "page_document",
                        principalColumn: "page_document_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment_term_translation",
                columns: table => new
                {
                    payment_term_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_term_translation_scope = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    payment_term_translation_schedule = table.Column<string>(type: "text", nullable: true),
                    payment_term_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    payment_term_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    payment_term_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    payment_term_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    payment_term_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    payment_term_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    payment_term_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    payment_term_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_term_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    payment_term_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    payment_term_translation_status = table.Column<string>(type: "text", nullable: false),
                    payment_term_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_term_translation", x => x.payment_term_translation_id);
                    table.ForeignKey(
                        name: "FK_payment_term_translation_payment_term_payment_term_translat~",
                        column: x => x.payment_term_translation_parentid,
                        principalTable: "payment_term",
                        principalColumn: "payment_term_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "process_step_translation",
                columns: table => new
                {
                    process_step_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    process_step_translation_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    process_step_translation_duration = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    process_step_translation_summary = table.Column<string>(type: "text", nullable: true),
                    process_step_translation_details = table.Column<string>(type: "jsonb", nullable: true),
                    process_step_translation_clientinput = table.Column<string>(type: "text", nullable: true),
                    process_step_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    process_step_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    process_step_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    process_step_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    process_step_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    process_step_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    process_step_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    process_step_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    process_step_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    process_step_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    process_step_translation_status = table.Column<string>(type: "text", nullable: false),
                    process_step_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process_step_translation", x => x.process_step_translation_id);
                    table.ForeignKey(
                        name: "FK_process_step_translation_process_step_process_step_translat~",
                        column: x => x.process_step_translation_parentid,
                        principalTable: "process_step",
                        principalColumn: "process_step_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_phase_translation",
                columns: table => new
                {
                    project_phase_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_phase_translation_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    project_phase_translation_price = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    project_phase_translation_duration = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    project_phase_translation_scope = table.Column<string>(type: "text", nullable: true),
                    project_phase_translation_note = table.Column<string>(type: "text", nullable: true),
                    project_phase_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    project_phase_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    project_phase_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    project_phase_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    project_phase_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    project_phase_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    project_phase_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    project_phase_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    project_phase_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    project_phase_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    project_phase_translation_status = table.Column<string>(type: "text", nullable: false),
                    project_phase_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_phase_translation", x => x.project_phase_translation_id);
                    table.ForeignKey(
                        name: "FK_project_phase_translation_project_phase_project_phase_trans~",
                        column: x => x.project_phase_translation_parentid,
                        principalTable: "project_phase",
                        principalColumn: "project_phase_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_package_translation",
                columns: table => new
                {
                    service_package_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_package_translation_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    service_package_translation_audience = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    service_package_translation_summary = table.Column<string>(type: "text", nullable: true),
                    service_package_translation_pricenote = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    service_package_translation_duration = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    service_package_translation_features = table.Column<string>(type: "jsonb", nullable: true),
                    service_package_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    service_package_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    service_package_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    service_package_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    service_package_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_package_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    service_package_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    service_package_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    service_package_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    service_package_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    service_package_translation_status = table.Column<string>(type: "text", nullable: false),
                    service_package_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_package_translation", x => x.service_package_translation_id);
                    table.ForeignKey(
                        name: "FK_service_package_translation_service_package_service_package~",
                        column: x => x.service_package_translation_parentid,
                        principalTable: "service_package",
                        principalColumn: "service_package_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "site_profile_translation",
                columns: table => new
                {
                    site_profile_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_profile_translation_tagline = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    site_profile_translation_description = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    site_profile_translation_city = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    site_profile_translation_region = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    site_profile_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    site_profile_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    site_profile_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    site_profile_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    site_profile_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    site_profile_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    site_profile_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    site_profile_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    site_profile_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    site_profile_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    site_profile_translation_status = table.Column<string>(type: "text", nullable: false),
                    site_profile_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_profile_translation", x => x.site_profile_translation_id);
                    table.ForeignKey(
                        name: "FK_site_profile_translation_site_profile_site_profile_translat~",
                        column: x => x.site_profile_translation_parentid,
                        principalTable: "site_profile",
                        principalColumn: "site_profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_refreshtoken",
                columns: table => new
                {
                    user_refreshtoken_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_refreshtoken_userid = table.Column<Guid>(type: "uuid", nullable: true),
                    user_refreshtoken_token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    user_refreshtoken_expiresat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_refreshtoken_absoluteexpiresat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_refreshtoken_isrevoked = table.Column<bool>(type: "boolean", nullable: true),
                    user_refreshtoken_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    user_refreshtoken_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_refreshtoken_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_refreshtoken_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_refreshtoken_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_refreshtoken_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_refreshtoken_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_refreshtoken", x => x.user_refreshtoken_id);
                    table.ForeignKey(
                        name: "FK_user_refreshtoken_user_user_refreshtoken_userid",
                        column: x => x.user_refreshtoken_userid,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "case_study_translation",
                columns: table => new
                {
                    case_study_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    case_study_translation_title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    case_study_translation_summary = table.Column<string>(type: "text", nullable: true),
                    case_study_translation_problem = table.Column<string>(type: "text", nullable: true),
                    case_study_translation_client = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    case_study_translation_kind = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    case_study_translation_duration = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    case_study_translation_role = table.Column<string>(type: "text", nullable: true),
                    case_study_translation_coveralt = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    case_study_translation_metrics = table.Column<string>(type: "jsonb", nullable: true),
                    case_study_translation_body = table.Column<string>(type: "jsonb", nullable: true),
                    case_study_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    case_study_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    case_study_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    case_study_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    case_study_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    case_study_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    case_study_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    case_study_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    case_study_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    case_study_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    case_study_translation_status = table.Column<string>(type: "text", nullable: false),
                    case_study_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_study_translation", x => x.case_study_translation_id);
                    table.ForeignKey(
                        name: "FK_case_study_translation_case_study_case_study_translation_pa~",
                        column: x => x.case_study_translation_parentid,
                        principalTable: "case_study",
                        principalColumn: "case_study_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    service_startingprice = table.Column<decimal>(type: "numeric", nullable: false),
                    service_pricingshape = table.Column<string>(type: "text", nullable: false),
                    service_casestudyid = table.Column<Guid>(type: "uuid", nullable: true),
                    service_order = table.Column<int>(type: "integer", nullable: false),
                    service_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    service_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    service_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    service_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    service_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    service_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.service_id);
                    table.ForeignKey(
                        name: "FK_service_case_study_service_casestudyid",
                        column: x => x.service_casestudyid,
                        principalTable: "case_study",
                        principalColumn: "case_study_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "role_menu",
                columns: table => new
                {
                    role_menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_menu_role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_menu_menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_menu_canview = table.Column<bool>(type: "boolean", nullable: false),
                    role_menu_cancreate = table.Column<bool>(type: "boolean", nullable: false),
                    role_menu_canupdate = table.Column<bool>(type: "boolean", nullable: false),
                    role_menu_candelete = table.Column<bool>(type: "boolean", nullable: false),
                    role_menu_canverify = table.Column<bool>(type: "boolean", nullable: false),
                    role_menu_customevents = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    role_menu_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    role_menu_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    role_menu_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role_menu_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    role_menu_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    role_menu_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    role_menu_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_menu", x => x.role_menu_id);
                    table.ForeignKey(
                        name: "FK_role_menu_menu_role_menu_menu_id",
                        column: x => x.role_menu_menu_id,
                        principalTable: "menu",
                        principalColumn: "menu_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_menu_role_role_menu_role_id",
                        column: x => x.role_menu_role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    user_role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_role_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_role_role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_role_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    user_role_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_role_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_role_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_role_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_role_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_role_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => x.user_role_id);
                    table.ForeignKey(
                        name: "FK_user_role_role_user_role_role_id",
                        column: x => x.user_role_role_id,
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_user_user_role_user_id",
                        column: x => x.user_role_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_translation",
                columns: table => new
                {
                    service_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_translation_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    service_translation_shortname = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    service_translation_audience = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    service_translation_headline = table.Column<string>(type: "text", nullable: true),
                    service_translation_summary = table.Column<string>(type: "text", nullable: true),
                    service_translation_startingpricelabel = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    service_translation_problems = table.Column<string>(type: "jsonb", nullable: true),
                    service_translation_deliverables = table.Column<string>(type: "jsonb", nullable: true),
                    service_translation_exclusions = table.Column<string>(type: "jsonb", nullable: true),
                    service_translation_faq = table.Column<string>(type: "jsonb", nullable: true),
                    service_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    service_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    service_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    service_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    service_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    service_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    service_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    service_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    service_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    service_translation_status = table.Column<string>(type: "text", nullable: false),
                    service_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_translation", x => x.service_translation_id);
                    table.ForeignKey(
                        name: "FK_service_translation_service_service_translation_parentid",
                        column: x => x.service_translation_parentid,
                        principalTable: "service",
                        principalColumn: "service_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vertical",
                columns: table => new
                {
                    vertical_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vertical_contentkey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    vertical_serviceid = table.Column<Guid>(type: "uuid", nullable: true),
                    vertical_pricingshape = table.Column<string>(type: "text", nullable: false),
                    vertical_order = table.Column<int>(type: "integer", nullable: false),
                    vertical_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    vertical_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    vertical_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    vertical_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    vertical_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    vertical_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    vertical_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vertical", x => x.vertical_id);
                    table.ForeignKey(
                        name: "FK_vertical_service_vertical_serviceid",
                        column: x => x.vertical_serviceid,
                        principalTable: "service",
                        principalColumn: "service_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vertical_translation",
                columns: table => new
                {
                    vertical_translation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vertical_translation_industry = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    vertical_translation_headline = table.Column<string>(type: "text", nullable: true),
                    vertical_translation_intro = table.Column<string>(type: "text", nullable: true),
                    vertical_translation_note = table.Column<string>(type: "text", nullable: true),
                    vertical_translation_whatsappintro = table.Column<string>(type: "text", nullable: true),
                    vertical_translation_problems = table.Column<string>(type: "jsonb", nullable: true),
                    vertical_translation_deliverables = table.Column<string>(type: "jsonb", nullable: true),
                    vertical_translation_faq = table.Column<string>(type: "jsonb", nullable: true),
                    vertical_translation_isdeleted = table.Column<bool>(type: "boolean", nullable: false),
                    vertical_translation_createddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    vertical_translation_updateddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    vertical_translation_deleteddate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    vertical_translation_createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    vertical_translation_updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    vertical_translation_deletedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    vertical_translation_parentid = table.Column<Guid>(type: "uuid", nullable: false),
                    vertical_translation_localecode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    vertical_translation_slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    vertical_translation_status = table.Column<string>(type: "text", nullable: false),
                    vertical_translation_publishedat = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vertical_translation", x => x.vertical_translation_id);
                    table.ForeignKey(
                        name: "FK_vertical_translation_vertical_vertical_translation_parentid",
                        column: x => x.vertical_translation_parentid,
                        principalTable: "vertical",
                        principalColumn: "vertical_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_add_on_add_on_isdeleted_add_on_order",
                table: "add_on",
                columns: new[] { "add_on_isdeleted", "add_on_order" });

            migrationBuilder.CreateIndex(
                name: "IX_add_on_translation_add_on_translation_localecode_add_on_tra~",
                table: "add_on_translation",
                columns: new[] { "add_on_translation_localecode", "add_on_translation_slug", "add_on_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_add_on_translation_add_on_translation_parentid_add_on_trans~",
                table: "add_on_translation",
                columns: new[] { "add_on_translation_parentid", "add_on_translation_localecode" },
                unique: true,
                filter: "\"add_on_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_case_study_case_study_contentkey",
                table: "case_study",
                column: "case_study_contentkey");

            migrationBuilder.CreateIndex(
                name: "IX_case_study_case_study_covermediaid",
                table: "case_study",
                column: "case_study_covermediaid");

            migrationBuilder.CreateIndex(
                name: "IX_case_study_case_study_isdeleted_case_study_order",
                table: "case_study",
                columns: new[] { "case_study_isdeleted", "case_study_order" });

            migrationBuilder.CreateIndex(
                name: "IX_case_study_translation_case_study_translation_localecode_ca~",
                table: "case_study_translation",
                columns: new[] { "case_study_translation_localecode", "case_study_translation_slug", "case_study_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_case_study_translation_case_study_translation_parentid_case~",
                table: "case_study_translation",
                columns: new[] { "case_study_translation_parentid", "case_study_translation_localecode" },
                unique: true,
                filter: "\"case_study_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_locale_locale_isdeleted_locale_code",
                table: "locale",
                columns: new[] { "locale_isdeleted", "locale_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_locale_locale_isdeleted_locale_isactive_locale_order",
                table: "locale",
                columns: new[] { "locale_isdeleted", "locale_isactive", "locale_order" });

            migrationBuilder.CreateIndex(
                name: "IX_media_media_isdeleted_media_createddate",
                table: "media",
                columns: new[] { "media_isdeleted", "media_createddate" });

            migrationBuilder.CreateIndex(
                name: "IX_menu_menu_isdeleted_menu_code",
                table: "menu",
                columns: new[] { "menu_isdeleted", "menu_code" });

            migrationBuilder.CreateIndex(
                name: "IX_note_note_contentkey",
                table: "note",
                column: "note_contentkey");

            migrationBuilder.CreateIndex(
                name: "IX_note_note_isdeleted_note_order",
                table: "note",
                columns: new[] { "note_isdeleted", "note_order" });

            migrationBuilder.CreateIndex(
                name: "IX_note_translation_note_translation_localecode_note_translati~",
                table: "note_translation",
                columns: new[] { "note_translation_localecode", "note_translation_slug", "note_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_note_translation_note_translation_parentid_note_translation~",
                table: "note_translation",
                columns: new[] { "note_translation_parentid", "note_translation_localecode" },
                unique: true,
                filter: "\"note_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_page_copy_page_copy_isdeleted_page_copy_pagecode",
                table: "page_copy",
                columns: new[] { "page_copy_isdeleted", "page_copy_pagecode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_page_copy_slot_definition_page_copy_slot_definition_isdelet~",
                table: "page_copy_slot_definition",
                columns: new[] { "page_copy_slot_definition_isdeleted", "page_copy_slot_definition_pagecode", "page_copy_slot_definition_order" });

            migrationBuilder.CreateIndex(
                name: "IX_page_copy_slot_definition_page_copy_slot_definition_pagecod~",
                table: "page_copy_slot_definition",
                columns: new[] { "page_copy_slot_definition_pagecode", "page_copy_slot_definition_slotkey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_page_copy_translation_page_copy_translation_localecode_page~",
                table: "page_copy_translation",
                columns: new[] { "page_copy_translation_localecode", "page_copy_translation_slug", "page_copy_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_page_copy_translation_page_copy_translation_parentid_page_c~",
                table: "page_copy_translation",
                columns: new[] { "page_copy_translation_parentid", "page_copy_translation_localecode" },
                unique: true,
                filter: "\"page_copy_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_page_document_page_document_isdeleted_page_document_pagecode",
                table: "page_document",
                columns: new[] { "page_document_isdeleted", "page_document_pagecode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_page_document_translation_page_document_translation_localec~",
                table: "page_document_translation",
                columns: new[] { "page_document_translation_localecode", "page_document_translation_slug", "page_document_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_page_document_translation_page_document_translation_parenti~",
                table: "page_document_translation",
                columns: new[] { "page_document_translation_parentid", "page_document_translation_localecode" },
                unique: true,
                filter: "\"page_document_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_payment_term_payment_term_isdeleted_payment_term_order",
                table: "payment_term",
                columns: new[] { "payment_term_isdeleted", "payment_term_order" });

            migrationBuilder.CreateIndex(
                name: "IX_payment_term_translation_payment_term_translation_localecod~",
                table: "payment_term_translation",
                columns: new[] { "payment_term_translation_localecode", "payment_term_translation_slug", "payment_term_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_payment_term_translation_payment_term_translation_parentid_~",
                table: "payment_term_translation",
                columns: new[] { "payment_term_translation_parentid", "payment_term_translation_localecode" },
                unique: true,
                filter: "\"payment_term_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_process_step_process_step_isdeleted_process_step_order",
                table: "process_step",
                columns: new[] { "process_step_isdeleted", "process_step_order" });

            migrationBuilder.CreateIndex(
                name: "IX_process_step_translation_process_step_translation_localecod~",
                table: "process_step_translation",
                columns: new[] { "process_step_translation_localecode", "process_step_translation_slug", "process_step_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_process_step_translation_process_step_translation_parentid_~",
                table: "process_step_translation",
                columns: new[] { "process_step_translation_parentid", "process_step_translation_localecode" },
                unique: true,
                filter: "\"process_step_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_project_phase_project_phase_isdeleted_project_phase_order",
                table: "project_phase",
                columns: new[] { "project_phase_isdeleted", "project_phase_order" });

            migrationBuilder.CreateIndex(
                name: "IX_project_phase_translation_project_phase_translation_localec~",
                table: "project_phase_translation",
                columns: new[] { "project_phase_translation_localecode", "project_phase_translation_slug", "project_phase_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_project_phase_translation_project_phase_translation_parenti~",
                table: "project_phase_translation",
                columns: new[] { "project_phase_translation_parentid", "project_phase_translation_localecode" },
                unique: true,
                filter: "\"project_phase_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_role_role_defaultmenu_id",
                table: "role",
                column: "role_defaultmenu_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_role_isdeleted_role_code",
                table: "role",
                columns: new[] { "role_isdeleted", "role_code" });

            migrationBuilder.CreateIndex(
                name: "IX_role_menu_role_menu_isdeleted_role_menu_role_id_role_menu_m~",
                table: "role_menu",
                columns: new[] { "role_menu_isdeleted", "role_menu_role_id", "role_menu_menu_id" });

            migrationBuilder.CreateIndex(
                name: "IX_role_menu_role_menu_menu_id",
                table: "role_menu",
                column: "role_menu_menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_menu_role_menu_role_id",
                table: "role_menu",
                column: "role_menu_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_service_casestudyid",
                table: "service",
                column: "service_casestudyid");

            migrationBuilder.CreateIndex(
                name: "IX_service_service_contentkey",
                table: "service",
                column: "service_contentkey");

            migrationBuilder.CreateIndex(
                name: "IX_service_service_isdeleted_service_order",
                table: "service",
                columns: new[] { "service_isdeleted", "service_order" });

            migrationBuilder.CreateIndex(
                name: "IX_service_package_service_package_isdeleted_service_package_g~",
                table: "service_package",
                columns: new[] { "service_package_isdeleted", "service_package_group", "service_package_order" });

            migrationBuilder.CreateIndex(
                name: "IX_service_package_translation_service_package_translation_loc~",
                table: "service_package_translation",
                columns: new[] { "service_package_translation_localecode", "service_package_translation_slug", "service_package_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_service_package_translation_service_package_translation_par~",
                table: "service_package_translation",
                columns: new[] { "service_package_translation_parentid", "service_package_translation_localecode" },
                unique: true,
                filter: "\"service_package_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_service_translation_service_translation_localecode_service_~",
                table: "service_translation",
                columns: new[] { "service_translation_localecode", "service_translation_slug", "service_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_service_translation_service_translation_parentid_service_tr~",
                table: "service_translation",
                columns: new[] { "service_translation_parentid", "service_translation_localecode" },
                unique: true,
                filter: "\"service_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_site_profile_translation_site_profile_translation_localecod~",
                table: "site_profile_translation",
                columns: new[] { "site_profile_translation_localecode", "site_profile_translation_slug", "site_profile_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_site_profile_translation_site_profile_translation_parentid_~",
                table: "site_profile_translation",
                columns: new[] { "site_profile_translation_parentid", "site_profile_translation_localecode" },
                unique: true,
                filter: "\"site_profile_translation_isdeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_slug_history_slug_history_entitytype_slug_history_localecod~",
                table: "slug_history",
                columns: new[] { "slug_history_entitytype", "slug_history_localecode", "slug_history_oldslug" });

            migrationBuilder.CreateIndex(
                name: "IX_user_user_isdeleted_user_email",
                table: "user",
                columns: new[] { "user_isdeleted", "user_email" });

            migrationBuilder.CreateIndex(
                name: "IX_user_refreshtoken_user_refreshtoken_isdeleted_user_refresht~",
                table: "user_refreshtoken",
                columns: new[] { "user_refreshtoken_isdeleted", "user_refreshtoken_token", "user_refreshtoken_isrevoked" });

            migrationBuilder.CreateIndex(
                name: "IX_user_refreshtoken_user_refreshtoken_userid",
                table: "user_refreshtoken",
                column: "user_refreshtoken_userid");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_user_role_isdeleted_user_role_user_id_user_role_r~",
                table: "user_role",
                columns: new[] { "user_role_isdeleted", "user_role_user_id", "user_role_role_id" });

            migrationBuilder.CreateIndex(
                name: "IX_user_role_user_role_role_id",
                table: "user_role",
                column: "user_role_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_user_role_user_id",
                table: "user_role",
                column: "user_role_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_vertical_vertical_contentkey",
                table: "vertical",
                column: "vertical_contentkey");

            migrationBuilder.CreateIndex(
                name: "IX_vertical_vertical_isdeleted_vertical_order",
                table: "vertical",
                columns: new[] { "vertical_isdeleted", "vertical_order" });

            migrationBuilder.CreateIndex(
                name: "IX_vertical_vertical_serviceid",
                table: "vertical",
                column: "vertical_serviceid");

            migrationBuilder.CreateIndex(
                name: "IX_vertical_translation_vertical_translation_localecode_vertic~",
                table: "vertical_translation",
                columns: new[] { "vertical_translation_localecode", "vertical_translation_slug", "vertical_translation_status" });

            migrationBuilder.CreateIndex(
                name: "IX_vertical_translation_vertical_translation_parentid_vertical~",
                table: "vertical_translation",
                columns: new[] { "vertical_translation_parentid", "vertical_translation_localecode" },
                unique: true,
                filter: "\"vertical_translation_isdeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "add_on_translation");

            migrationBuilder.DropTable(
                name: "case_study_translation");

            migrationBuilder.DropTable(
                name: "locale");

            migrationBuilder.DropTable(
                name: "note_translation");

            migrationBuilder.DropTable(
                name: "page_copy_slot_definition");

            migrationBuilder.DropTable(
                name: "page_copy_translation");

            migrationBuilder.DropTable(
                name: "page_document_translation");

            migrationBuilder.DropTable(
                name: "payment_term_translation");

            migrationBuilder.DropTable(
                name: "process_step_translation");

            migrationBuilder.DropTable(
                name: "project_phase_translation");

            migrationBuilder.DropTable(
                name: "role_menu");

            migrationBuilder.DropTable(
                name: "service_package_translation");

            migrationBuilder.DropTable(
                name: "service_translation");

            migrationBuilder.DropTable(
                name: "site_profile_translation");

            migrationBuilder.DropTable(
                name: "slug_history");

            migrationBuilder.DropTable(
                name: "user_refreshtoken");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "vertical_translation");

            migrationBuilder.DropTable(
                name: "add_on");

            migrationBuilder.DropTable(
                name: "note");

            migrationBuilder.DropTable(
                name: "page_copy");

            migrationBuilder.DropTable(
                name: "page_document");

            migrationBuilder.DropTable(
                name: "payment_term");

            migrationBuilder.DropTable(
                name: "process_step");

            migrationBuilder.DropTable(
                name: "project_phase");

            migrationBuilder.DropTable(
                name: "service_package");

            migrationBuilder.DropTable(
                name: "site_profile");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "vertical");

            migrationBuilder.DropTable(
                name: "menu");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "case_study");

            migrationBuilder.DropTable(
                name: "media");
        }
    }
}
